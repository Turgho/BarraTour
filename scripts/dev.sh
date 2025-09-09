#!/bin/bash
# dev.sh

echo "🚀 Iniciando Barra Tour em modo desenvolvimento..."

# Verifica se o Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Por favor, inicie o Docker Desktop."
    exit 1
fi

# Build e execução
docker-compose down
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build -d

echo "✅ Serviços iniciados:"
echo "   - API: http://localhost:5078"
echo "   - SQL Server: localhost:1433"
echo "   - Redis: localhost:6379"
echo ""
echo "📝 Para ver os logs: docker-compose logs -f api"