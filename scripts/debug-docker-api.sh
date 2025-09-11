#!/bin/bash
# run-docker-debug.sh - Script para DEBUG da API Barra Tour

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

# Função para construir as imagens
build_images() {
    echo "🏗️  Construindo imagens Docker..."
    
    # Verificar qual comando do Docker Compose está disponível
    if command -v docker-compose &> /dev/null; then
        COMPOSE_CMD="docker-compose"
    else
        COMPOSE_CMD="docker compose"
    fi
    
    if ! $COMPOSE_CMD build --no-cache; then
        echo "❌ Falha ao construir imagens Docker"
        return 1
    fi
    
    echo "✅ Imagens construídas com sucesso"
    return 0
}

# Função para DEBUG - Executar API e mostrar logs completos
debug_api() {
    echo "🐛 INICIANDO MODO DEBUG..."
    echo "=========================="
    
    # Verificar qual comando do Docker Compose está disponível
    if command -v docker-compose &> /dev/null; then
        COMPOSE_CMD="docker-compose"
    else
        COMPOSE_CMD="docker compose"
    fi
    
    # Parar tudo primeiro
    echo "🛑 Parando containers existentes..."
    $COMPOSE_CMD down
    
    # Construir imagens
    if ! build_images; then
        exit 1
    fi
    
    echo "🚀 Iniciando API em modo debug..."
    echo "📍 A API tentará iniciar em: http://localhost:5078"
    echo "📢 TODOS OS LOGS SERÃO MOSTRADOS (incluindo erros)"
    echo "🛑 Para parar, pressione Ctrl+C"
    echo ""
    echo "=================================================="
    
    # Executar API em foreground mostrando TODOS os logs
    $COMPOSE_CMD up api
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
echo "🐳 DEBUG - Barra Tour API"
echo "========================"

# Verificar ambiente primeiro
if ! check_environment; then
    echo "❌ Falha na verificação do ambiente. Abortando."
    exit 1
fi

# Configurar ambiente
setup_environment

# Executar debug
debug_api