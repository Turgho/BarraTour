# 🌊 BarraTour — Agendamento de Turismo em Barra Bonita

[![Status](https://img.shields.io/badge/status-em%20desenvolvimento-yellow)](https://github.com/Turgho/BarraTour)
[![Linguagem](https://img.shields.io/badge/Linguagem-C%23-blue)](https://docs.microsoft.com/dotnet/csharp/)
[![Último commit](https://img.shields.io/github/last-commit/Turgho/BarraTour)](https://github.com/Turgho/BarraTour/commits/main)
[![License: MIT](https://img.shields.io/badge/License-MIT-green)](./LICENSE)

**BarraTour** é uma PWA para agendamento de passeios e atividades em **Barra Bonita (SP)**. O projeto conecta turistas e empresas locais, permitindo descobrir atrações, criar reservas e — futuramente — processar pagamentos e comunicação em tempo real.

> **Status atual:** repositório público — desenvolvimento ativo.  
> **Escopo visível aqui:** somente o **MVP 1** (Cadastro, Login e perfis). A documentação completa está em `docs/`.

---

## 🎯 MVP 1 — Recursos disponíveis neste repositório
Implementação mínima viável para começar a usar e demonstrar o fluxo básico:

- ✅ **Cadastro de turista** (nome, email, senha, idioma preferido)
- ✅ **Cadastro de empresa** (nome, email, senha, CNPJ/CPF, setor)
- ✅ **Login** com autenticação JWT
- ✅ **Recuperação de senha** (fluxo básico)
- ✅ **Painéis distintos** (rotas/papéis): turista e empresa (visualizações iniciais)
- ✅ **Estrutura do banco** (SQL Server) e migrations iniciais (docs em `docs/05_BancoDeDados.md`)
- ✅ Documentação do projeto e wireframes em `docs/`

> Observação: funcionalidades como reservas completas, pagamentos e mensageria estão documentadas e planeadas (veja `docs/`), mas não estão ativas no MVP1 deste repositório público.

---

## 📂 Documentação (pasta `docs/`)
A documentação completa foi organizada em arquivos Markdown dentro de `docs/`. Navegue pelos arquivos para ver detalhes do projeto:

- `docs/01_Resumo.md` — Visão geral
- `docs/02_Escopo.md` — Escopo do produto
- `docs/03_Stack.md` — Tecnologias sugeridas
- `docs/04_Arquitetura.md` — Diagrama e arquitetura
- `docs/05_BancoDeDados.md` — Modelagem SQL Server (DDL)
- `docs/06_UserStories.md` — Backlog e user stories
- `docs/07_API.md` — Contratos e formato de retorno da API
- `docs/08_Fluxos.md` — Fluxos em Mermaid
- `docs/09_DesignUX.md` — Wireframes e UX
- `docs/10_Pagamentos.md` — Planejamento AbacatePay
- `docs/11_Mensageria.md` — Mensageria e notificações
- `docs/12_Segurança.md` — Políticas de segurança
- `docs/13_Testes.md` — Plano de testes
- `docs/14_Deploy&Infra.md` — Deploy e infra (CI/CD)
- `docs/15_Roadmap.md` — Sprints e roadmap

---

## 🧭 Tecnologias (visão do MVP)
- **Backend:** C# / .NET (API REST)
- **Frontend:** Angular (PWA) — mobile-first
- **DB:** SQL Server (modelagem pronta em `docs/05_BancoDeDados.md`)
- **Auth:** JWT (tokens de acesso e refresh)

> Obs.: outras integrações (SignalR, AbacatePay, RabbitMQ) estão descritas em `docs/` para fases posteriores.

---

## ✍️ Como contribuir
Se quiser contribuir (issues, PRs, feedback):
- Abra uma **issue** descrevendo a proposta ou bug.
- Faça fork, crie branch `feature/...` ou `fix/...` e abra PR contra `main`.
- Leia `CONTRIBUTING.md` e `CODE_OF_CONDUCT.md` (se houver) antes de submeter.

---

## 🔒 Segurança e boas práticas
- **Nenhuma credencial sensível** deve ser commitada.
- Use secrets do CI para chaves (se houver).
- Se encontrar vulnerabilidade, reporte para o contato indicado em `SECURITY.md`.

---

## ⚡ Contato
- Autor / Maintainer: **Turgho** — perfil no GitHub: [Turgho](https://github.com/Turgho)
- Para sugestões ou dúvidas, abra uma **issue** no repositório.

---

Obrigado por visitar o **BarraTour** — se quiser, posso ajudar a gerar os `docs/*.md` finais, criar badges dinâmicos para CI, ou preparar um *demo* público (deploy) quando o MVP1 estiver pronto. Quer que eu gere algum arquivo `docs/` agora?
