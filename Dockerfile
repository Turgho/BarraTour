# Dockerfile.dev
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble

# Define variáveis de ambiente
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_URLS=http://+:80;https://+:443

# Instala ferramentas úteis para desenvolvimento
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    curl \
    procps \
    && rm -rf /var/lib/apt/lists/*

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos de projeto primeiro (para melhor cache)
COPY ["BarraTour.sln", "."]
COPY ["src/BarraTour.Api/BarraTour.Api.csproj", "src/BarraTour.Api/"]
COPY ["src/BarraTour.Application/BarraTour.Application.csproj", "src/BarraTour.Application/"]
COPY ["src/BarraTour.Domain/BarraTour.Domain.csproj", "src/BarraTour.Domain/"]
COPY ["src/BarraTour.Infrastructure/BarraTour.Infrastructure.csproj", "src/BarraTour.Infrastructure/"]

# Restaura as dependências
RUN dotnet restore "BarraTour.sln"

# Copia todo o código fonte
COPY . .

# Expõe as portas
EXPOSE 80
EXPOSE 443

# Comando para desenvolvimento (watch mode)
ENTRYPOINT ["dotnet", "watch", "--project", "src/BarraTour.Api/", "run", "--urls", "http://0.0.0.0:80;https://0.0.0.0:443"]