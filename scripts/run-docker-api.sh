#!/bin/bash
# run-docker-api.sh - Script para executar a API Barra Tour com Docker Compose e Hot Reload

# Função para verificar dependências
check_environment() {
    echo "🔍 Verificando dependências do sistema..."
    echo "----------------------------------------"

    # Verificar Docker
    if ! command -v docker &> /dev/null; then
        echo "❌ Docker não encontrado!"
        echo "   Instale em: https://docs.docker.com/get-docker/"
        return 1
    fi
    
    # Verificar Docker Compose
    if ! command -v docker-compose &> /dev/null && ! command -v docker compose &> /dev/null; then
        echo "❌ Docker Compose não encontrado!"
        echo "   Instale em: https://docs.docker.com/compose/install/"
        return 1
    fi

    docker_version=$(docker --version)
    echo "✅ Docker: $docker_version"

    # Verificar se o Docker está rodando
    if ! docker info > /dev/null 2>&1; then
        echo "❌ Docker não está rodando. Por favor, inicie o Docker Desktop."
        return 1
    fi
    echo "✅ Docker está rodando"

    # Verificar arquivos do Docker
    if [ ! -f "docker-compose.yml" ]; then
        echo "❌ docker-compose.yml não encontrado!"
        return 1
    fi
    echo "✅ docker-compose.yml: OK"

    # Verificar Dockerfile.dev
    if [ ! -f "Dockerfile.dev" ]; then
        echo "❌ Dockerfile.dev não encontrado!"
        return 1
    fi
    echo "✅ Dockerfile.dev: OK"

    echo "----------------------------------------"
    echo "✅ Todas as verificações passaram!"
    return 0
}

# Função para configurar variáveis de ambiente
setup_environment() {
    echo "🌿 Configurando ambiente Docker..."
    
    # Carregar variáveis de ambiente do arquivo .env se existir
    if [ -f ".env" ]; then
        echo "📁 Carregando variáveis do arquivo .env"
        export $(cat .env | grep -v '#' | awk '/=/ {print $1}')
    else
        echo "⚠️  Arquivo .env não encontrado, usando valores padrão"
        # Valores padrão para desenvolvimento
        export SA_PASSWORD=Barra@Tour3002
        export REDIS_PASSWORD=BarraRedis3002
        export JWT_SECRET_KEY=DevelopmentSuperSecretKeyThatIsLongEnoughForSecurity
        export JWT_ISSUER=BarraTour.API
        export JWT_AUDIENCE=BarraTour.App
        export JWT_EXPIRY_IN_MINUTES=60
    fi
    
    # Variáveis de ambiente padrão para Docker
    export COMPOSE_PROJECT_NAME=barra-tour
    export ASPNETCORE_ENVIRONMENT=Development
    
    echo "✅ Ambiente Docker configurado"
}

# Função para aguardar até que um serviço esteja healthy
wait_for_service() {
    local service_name=$1
    local max_attempts=30
    local attempt=1
    
    echo "⏳ Aguardando serviço $service_name ficar healthy..."
    
    while [ $attempt -le $max_attempts ]; do
        local status=$($COMPOSE_CMD ps -q $service_name | xargs docker inspect --format='{{.State.Health.Status}}' 2>/dev/null)
        
        if [ "$status" = "healthy" ]; then
            echo "✅ Serviço $service_name está healthy"
            return 0
        fi
        
        echo "⚠️  Serviço $service_name ainda não está healthy (tentativa $attempt/$max_attempts). Status: $status"
        attempt=$((attempt + 1))
        sleep 5
    done
    
    echo "❌ Serviço $service_name não ficou healthy após $max_attempts tentativas"
    return 1
}

# Função para construir as imagens
build_images() {
    echo "🏗️  Construindo imagens Docker..."
    
    # Verificar qual comando do Docker Compose está disponível
    if command -v docker-compose &> /dev/null; then
        COMPOSE_CMD="docker-compose"
    else
        COMPOSE_CMD="docker compose"
    fi
    
    if ! $COMPOSE_CMD build; then
        echo "❌ Falha ao construir imagens Docker"
        return 1
    fi
    
    echo "✅ Imagens construídas com sucesso"
    return 0
}

# Função para iniciar os serviços em background (SQL Server e Redis)
start_background_services() {
    echo "🚀 Iniciando serviços de background (SQL Server e Redis)..."
    
    # Verificar qual comando do Docker Compose está disponível
    if command -v docker-compose &> /dev/null; then
        COMPOSE_CMD="docker-compose"
    else
        COMPOSE_CMD="docker compose"
    fi
    
    # Iniciar apenas SQL Server e Redis em background
    if ! $COMPOSE_CMD up -d sqlserver redis; then
        echo "❌ Falha ao iniciar serviços de background"
        return 1
    fi
    
    # Aguardar serviços de background ficarem saudáveis
    echo "⏳ Aguardando serviços de background ficarem prontos..."
    sleep 20  # Espera inicial para os serviços começarem
    
    # Esperar pelo SQL Server
    if ! wait_for_service "sqlserver"; then
        echo "❌ SQL Server não ficou healthy a tempo"
        return 1
    fi
    
    # Esperar pelo Redis
    if ! wait_for_service "redis"; then
        echo "❌ Redis não ficou healthy a tempo"
        return 1
    fi
    
    echo "✅ Serviços de background iniciados e healthy"
    return 0
}

# Função para iniciar a API em primeiro plano com hot reload
start_api_foreground() {
    echo "🚀 Iniciando API em primeiro plano com Hot Reload..."
    
    # Verificar qual comando do Docker Compose está disponível
    if command -v docker-compose &> /dev/null; then
        COMPOSE_CMD="docker-compose"
    else
        COMPOSE_CMD="docker compose"
    fi
    
    echo "📍 API estará disponível em: http://localhost:5078"
    echo "📍 Swagger UI: http://localhost:5078/swagger"
    echo "🔧 Hot Reload ativado - alterações no código serão refletidas automaticamente"
    echo "🛑 Para parar, pressione Ctrl+C"
    echo ""
    
    # Iniciar API em primeiro plano (isso mantém o processo rodando)
    $COMPOSE_CMD up api
}

# Função principal
main() {
    echo "🐳 Iniciando Barra Tour com Docker e Hot Reload..."
    echo "=================================================="

    # Verificar ambiente primeiro
    if ! check_environment; then
        echo "❌ Falha na verificação do ambiente. Abortando."
        exit 1
    fi

    # Configurar ambiente
    setup_environment

    # Construir imagens
    if ! build_images; then
        exit 1
    fi

    # Iniciar serviços de background
    if ! start_background_services; then
        exit 1
    fi

    # Iniciar API em primeiro plano
    start_api_foreground
}

# Tratamento de sinais para limpeza
cleanup() {
    echo ""
    echo "🛑 Parando serviços Docker..."
    
    # Verificar qual comando do Docker Compose está disponível
    if command -v docker-compose &> /dev/null; then
        COMPOSE_CMD="docker-compose"
    else
        COMPOSE_CMD="docker compose"
    fi
    
    $COMPOSE_CMD down
    echo "👋 Serviços parados. Até logo!"
    exit 0
}

# Configurar trap para Ctrl+C
trap cleanup INT TERM

# Executar função principal
main