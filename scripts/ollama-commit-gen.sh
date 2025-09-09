#!/bin/bash

# Verifica se o Ollama está instalado e acessível
if ! command -v ollama &> /dev/null; then
    echo "Erro: Ollama não está instalado ou não está no PATH."
    exit 1
fi

# Verifica se o repositório é Git
if ! git rev-parse --is-inside-work-tree > /dev/null 2>&1; then
    echo "Erro: Este diretório não é um repositório Git."
    exit 1
fi

# Obtém as mudanças atuais
CHANGES=$(git diff --staged --name-status)
if [ -z "$CHANGES" ]; then
    echo "Nenhuma alteração staged encontrada. Use 'git add' primeiro."
    exit 1
fi

# Prepara o prompt para o Ollama com instruções específicas
PROMPT="Gere uma mensagem de commit concisa em português seguindo o Conventional Commits.
Use prefixos convencionais: feat, fix, docs, style, refactor, test, chore, perf, ci, build.
Exemplos: 
- feat: adiciona funcionalidade X
- fix: corrige problema Y
- docs: atualiza documentação Z
Para estas mudanças:\n$CHANGES\n\nMensagem:"

# Gera a mensagem de commit usando o Ollama
COMMIT_MSG=$(ollama run llama3.1:8b "$PROMPT" | \
    grep -v "^>>>" | \
    head -n 1 | \
    sed 's/^"//;s/"$//' | \
    sed 's/^[[:space:]]*//;s/[[:space:]]*$//')

# Verifica se a mensagem foi gerada
if [ -z "$COMMIT_MSG" ]; then
    echo "Falha ao gerar a mensagem de commit."
    exit 1
fi

# Mostra a mensagem gerada para confirmação
echo "Mensagem de commit gerada:"
echo "$COMMIT_MSG"
echo ""
read -p "Confirmar commit? (s/N) " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Ss]$ ]]; then
    git commit -m "$COMMIT_MSG"
    echo "Commit realizado com a mensagem: $COMMIT_MSG"
else
    echo "Commit cancelado."
    exit 0
fi