```bash
BarraTour/
│
├── src/
│   ├── BarraTour.Backend/                 # Solução .NET Backend
│   │   ├── BarraTour.API/                 # Camada de Apresentação
│   │   │   ├── Controllers/               # Controladores REST
│   │   │   ├── Middlewares/               # Middlewares customizados
│   │   │   ├── Properties/                # Propriedades do projeto
│   │   │   ├── appsettings.json           # Configurações
│   │   │   └── Program.cs                 # Ponto de entrada
│   │   │
│   │   ├── BarraTour.Application/         # Camada de Aplicação
│   │   │   ├── Services/                  # Serviços de aplicação
│   │   │   ├── DTOs/                      # Objetos de transferência
│   │   │   ├── Interfaces/                # Interfaces de aplicação
│   │   │   ├── Validators/                # Validações (FluentValidation)
│   │   │   ├── Mappings/                  # Mapeamentos (AutoMapper)
│   │   │   └── Features/                  # Organização por funcionalidade
│   │   │
│   │   ├── BarraTour.Domain/              # Camada de Domínio
│   │   │   ├── Entities/                  # Entidades de domínio
│   │   │   ├── Enums/                     # Enumeradores
│   │   │   ├── ValueObjects/              # Objetos de valor
│   │   │   ├── Interfaces/                # Interfaces de domínio
│   │   │   └── Events/                    # Eventos de domínio
│   │   │
│   │   ├── BarraTour.Infrastructure/      # Camada de Infraestrutura
│   │   │   ├── Data/                      # Contexto do BD e configurações
│   │   │   ├── Repositories/              # Implementações de repositórios
│   │   │   ├── Services/                  # Serviços de infraestrutura
│   │   │   ├── Migrations/                # Migrações do EF Core
│   │   │   ├── Identity/                  # Autenticação e autorização
│   │   │   └── MessageBus/                # Mensageria (opcional)
│   │   │
│   │   └── BarraTour.Shared/              # Projeto compartilhado
│   │       ├── DTOs/                      # DTOs compartilhados
│   │       ├── Enums/                     # Enums compartilhados
│   │       ├── Constants/                 # Constantes compartilhadas
│   │       └── Helpers/                   # Utilitários compartilhados
│   │
│   └── BarraTour.Frontend/                # Frontend PWA (Angular/React/Vue)
│       ├── src/
│       │   ├── app/                       # Componentes principais
│       │   ├── assets/                    # Recursos estáticos
│       │   ├── environments/              # Ambientes de configuração
│       │   └── styles/                    # Estilos globais
│       └── package.json
│
├── tests/                                 # Testes automatizados
│   ├── BarraTour.API.Tests/               # Testes de integração da API
│   ├── BarraTour.Application.Tests/       # Testes de aplicação
│   ├── BarraTour.Domain.Tests/            # Testes de domínio
│   └── BarraTour.Infrastructure.Tests/    # Testes de infraestrutura
│
├── docs/                                  # Documentação
│   ├── database/                          # Esquemas de banco de dados
│   ├── api/                               # Documentação da API
│   └── architecture/                      # Diagramas de arquitetura
│
├── scripts/                               # Scripts auxiliares
│   ├── database/                          # Scripts de banco de dados
│   ├── deployment/                        # Scripts de implantação
│   └── utilities/                         # Utilitários diversos
│
├── docker/                                # Configurações do Docker
│   ├── compose.yml                        # Docker Compose
│   ├── Dockerfile.api                     # Dockerfile da API
│   └── Dockerfile.frontend                # Dockerfile do Frontend
│
├── .github/                               # Configurações do GitHub
│   └── workflows/                         # GitHub Actions CI/CD
│
├── .gitignore
├── .dockerignore
├── README.md
├── BarraTour.sln                          # Solução .NET
├── docker-compose.yml                     # Docker Compose principal
├── docker-compose.override.yml            # Docker Compose para desenvolvimento
└── Dockerfile                             # Dockerfile para build/development
```