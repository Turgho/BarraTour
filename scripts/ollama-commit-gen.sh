#!/bin/bash

# Configurações
CACHE_DIR="$HOME/.git_commit_cache"
CACHE_TTL=600 # 10 minutos em segundos
OLLAMA_MODEL="llama3.1:8b"

# Verifica dependências
if ! command -v ollama &> /dev/null; then
    echo "Error: Ollama is not installed."
    exit 1
fi

if ! git rev-parse --is-inside-work-tree > /dev/null 2>&1; then
    echo "Error: Not a git repository."
    exit 1
fi

# Cria diretório de cache se não existir
mkdir -p "$CACHE_DIR"

# Obtém arquivos staged
STAGED_FILES=$(git diff --staged --name-only)
if [ -z "$STAGED_FILES" ]; then
    echo "No staged files found."
    exit 1
fi

# Gera hash das mudanças para usar como chave de cache
CHANGE_HASH=$(git diff --staged --stat | sha256sum | cut -d' ' -f1)
CACHE_FILE="$CACHE_DIR/$CHANGE_HASH"

# Verifica se existe no cache
if [ -f "$CACHE_FILE" ]; then
    CACHE_TIME=$(stat -c %Y "$CACHE_FILE")
    CURRENT_TIME=$(date +%s)
    if [ $((CURRENT_TIME - CACHE_TIME)) -lt $CACHE_TTL ]; then
        COMMIT_MSG=$(cat "$CACHE_FILE")
        echo "Cached commit message: $COMMIT_MSG"
        echo "Files: $(echo "$STAGED_FILES" | wc -l) modified"
        read -p "Use this message? (Y/n) " -n 1 -r
        echo ""
        if [[ ! $REPLY =~ ^[Nn]$ ]]; then
            read -p "Confirm commit? (Y/n) " -n 1 -r
            echo ""
            if [[ ! $REPLY =~ ^[Nn]$ ]]; then
                git commit -m "$COMMIT_MSG" > /dev/null 2>&1
                echo "✅ Commit completed with cached message"
                exit 0
            else
                echo "❌ Commit canceled."
                exit 0
            fi
        fi
    fi
fi

# Prepara prompt com exemplos
CHANGE_SUMMARY=$(git diff --staged --stat)
PROMPT="Generate a concise commit message in English following Conventional Commits format.

Examples:
- feat: add user authentication module
- fix: resolve calculation error in tax formula
- docs: update API documentation
- refactor: improve database query performance
- style: format code according to guidelines
- test: add unit tests for user service
- chore: update dependencies
- perf: optimize image loading
- ci: configure GitHub Actions workflow
- build: update webpack configuration

Based on these changes:
$CHANGE_SUMMARY

Commit message:"

# Gera nova mensagem usando GPU
echo "Generating commit message with GPU..."
COMMIT_MSG=$(OLLAMA_NUM_GPU=1 ollama run --nowordwrap $OLLAMA_MODEL "$PROMPT" | \
    grep -E '^(feat|fix|docs|style|refactor|test|chore|perf|ci|build)' | \
    head -n 1 | \
    sed 's/^[[:space:]]*//;s/[[:space:]]*$//' | \
    sed 's/\.$//')

# Fallback se não encontrar padrão conventional
if [ -z "$COMMIT_MSG" ]; then
    COMMIT_MSG=$(OLLAMA_NUM_GPU=1 ollama run --nowordwrap $OLLAMA_MODEL "$PROMPT" | \
        grep -v "^>>>" | \
        head -n 1 | \
        sed 's/^[[:space:]]*//;s/[[:space:]]*$//' | \
        sed 's/\.$//')
    
    # Garante que tenha um prefixo válido
    if [[ ! "$COMMIT_MSG" =~ ^(feat|fix|docs|style|refactor|test|chore|perf|ci|build) ]]; then
        COMMIT_MSG="chore: $COMMIT_MSG"
    fi
fi

# Mostra a mensagem e pede confirmação
echo "Generated commit message: $COMMIT_MSG"
echo "Files to be committed:"
git diff --staged --name-only | sed 's/^/  /'
echo ""

read -p "Confirm commit with this message? (Y/n) " -n 1 -r
echo ""

if [[ ! $REPLY =~ ^[Nn]$ ]]; then
    # Salva no cache
    echo "$COMMIT_MSG" > "$CACHE_FILE"
    
    # Executa o commit
    git commit -m "$COMMIT_MSG" > /dev/null 2>&1
    echo "✅ Commit completed successfully"
else
    echo "❌ Commit canceled."
    exit 0
fi