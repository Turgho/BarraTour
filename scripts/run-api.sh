#!/bin/bash
# run-api.sh - Script para executar a API Barra Tour com verificação de ambiente

# Função para verificar dependências
check_environment() {
    echo "🔍 Verificando dependências do sistema..."
    echo "----------------------------------------"

    # Verificar .NET SDK
    if ! command -v dotnet &> /dev/null; then
        echo "❌ .NET SDK não encontrado!"
        echo "   Instale em: https://dotnet.microsoft.com/download"
        return 1
    fi
    
    dotnet_version=$(dotnet --version)
    echo "✅ .NET SDK: $dotnet_version"

    # Verificar estrutura de pastas
    if [ ! -d "../src/BarraTour.Backend/BarraTour.Api" ]; then
        echo "❌ Estrutura de pastas não encontrada"
        echo "   Esperado: src/BarraTour.Backend/BarraTour.Api"
        return 1
    fi
    echo "✅ Estrutura de pastas: OK"

    # Verificar arquivo de projeto
    if [ ! -f "../src/BarraTour.Backend/BarraTour.Api/BarraTour.Api.csproj" ]; then
        echo "❌ Arquivo de projeto não encontrado"
        return 1
    fi
    echo "✅ Arquivo de projeto: OK"

    # Verificar Docker (opcional, apenas informativo)
    if command -v docker &> /dev/null; then
        docker_version=$(docker --version)
        echo "✅ Docker: $docker_version (opcional)"
    else
        echo "⚠️  Docker não encontrado (opcional para desenvolvimento)"
    fi

    echo "----------------------------------------"
    echo "✅ Todas as verificações passaram!"
    return 0
}

# Função para configurar variáveis de ambiente
setup_environment() {
    echo "🌿 Configurando ambiente de desenvolvimento..."
    
    # Variáveis de ambiente principais
    export ASPNETCORE_ENVIRONMENT=Development
    export ConnectionStrings__DefaultConnection="Server=localhost;Database=BarraTourDb;User Id=sa;Password=Barra@Tour3002;TrustServerCertificate=true;"
    export ConnectionStrings__Redis="localhost:6379,password=BarraRedis3002,abortConnect=false"
    
    # Variáveis JWT (segurança)
    export Jwt__SecretKey="DevelopmentSuperSecretKeyThatIsLongEnoughForSecurity123!"
    export Jwt__Issuer="BarraTour.API"
    export Jwt__Audience="BarraTour.App"
    export Jwt__ExpiryInMinutes="60"
    
    echo "✅ Variáveis de ambiente configuradas"
}

# Função principal
main() {
    echo "🚀 Iniciando Barra Tour API..."
    echo "📂 Diretório: src/BarraTour.Backend/BarraTour.Api"
    echo "----------------------------------------"

    # Verificar ambiente primeiro
    if ! check_environment; then
        echo "❌ Falha na verificação do ambiente. Abortando."
        exit 1
    fi

    # Configurar ambiente
    setup_environment

    # Navegar para o diretório da API
    cd ../src/BarraTour.Backend/BarraTour.Api || {
        echo "❌ Falha ao navegar para o diretório da API"
        exit 1
    }

    echo "📦 Restaurando pacotes NuGet..."
    if ! dotnet restore; then
        echo "❌ Falha ao restaurar pacotes NuGet"
        exit 1
    fi

    echo "🔧 Compilando projeto..."
    if ! dotnet build; then
        echo "❌ Falha ao compilar projeto"
        exit 1
    fi

    echo "👀 Iniciando API em modo watch (hot reload)..."
    echo "📍 API estará disponível em: http://localhost:5078"
    echo "📍 Swagger UI: http://localhost:5078/swagger"
    echo "----------------------------------------"
    echo "🛑 Para parar a execução, pressione Ctrl+C"
    echo ""

    # Executar com watch mode
    dotnet watch run
}

# Tratamento de sinais para limpeza
cleanup() {
    echo ""
    echo "🔄 Restaurando diretório original..."
    cd - > /dev/null 2>&1
    echo "👋 Execução finalizada"
    exit 0
}

# Configurar trap para Ctrl+C
trap cleanup INT TERM

# Executar função principal
main