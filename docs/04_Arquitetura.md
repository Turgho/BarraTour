```bash
/TourismApp
│
├─ /src
│   ├─ /API                # Backend (.NET API)
│   │   ├─ /Controllers    # Endpoints REST
│   │   ├─ /Models         # Entidades e DTOs
│   │   ├─ /Data           # DbContext, migrations, seeds
│   │   ├─ /Repositories   # Acesso a dados
│   │   ├─ /Services       # Lógica de negócio (ex: pagamentos, reservas)
│   │   ├─ /Middlewares    # Autenticação, logging, error handling
│   │   ├─ /DTOs           # Objetos de transferência de dados
│   │   └─ Program.cs       # Inicialização da aplicação
│   │
│   ├─ /PWA                # Frontend PWA (Angular)
│   │   ├─ /components     # Componentes reutilizáveis
│   │   ├─ /pages          # Telas e rotas
│   │   ├─ /assets         # Imagens, CSS, ícones
│   │   ├─ /services       # Chamadas à API
│   │   └─ /store          # Estado global (Vuex / Redux)
│   │
│   └─ /Shared             # Código compartilhado entre backend e frontend
│       ├─ /Utils          # Funções utilitárias
│       ├─ /Enums          # Tipos de enum
│       └─ /Constants      # Constantes do sistema
│
├─ /docs                   # Documentação do projeto (Obsidian, diagramas)
│   ├─ MVPs.md
│   ├─ DB_Schema.md
│   ├─ Payment_Module.md
│   └─ ER_Diagram.md
│
├─ /tests                  # Testes unitários e integração
│   ├─ /API
│   └─ /PWA
│
├─ /scripts                # Scripts auxiliares (DB, seed, deploy)
│
├─ /docker                 # Dockerfiles e configs
│
├─ .gitignore
├─ README.md
└─ TourismApp.sln          # Solução .NET
```