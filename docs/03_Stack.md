### **Backend**

- **C# (.NET 8 ou superior)** → base do sistema.
    
- **ASP.NET Core Web API** → para fornecer os endpoints.
    
- **Entity Framework Core** → ORM para trabalhar com banco relacional.
    
- **AutoMapper** → facilitar conversão entre entidades e DTOs.
    
- **FluentValidation** → validação de dados de entrada.
    
- **MediatR** → se quiser usar CQRS para melhorar separação de responsabilidades.
    

---

### **Banco de Dados**

- **SQL Server ou PostgreSQL** (se for cloud, PostgreSQL tem bons planos gratuitos).
    
- **Redis** (opcional, mas bom para mostrar caching em consultas turísticas mais acessadas).
    

---

### **Frontend**

- **Angular com Tailwind** (se quiser mostrar diversidade).
    
- **PWA** habilitado → turistas podem instalar no celular sem precisar de app store.
    

---

### **Infraestrutura**

- **Docker** → conteinerizar backend, banco e frontend.
    
- **Docker Compose** → orquestrar localmente.
    
- **CI/CD (GitHub Actions)** → mostrar deploy automatizado.
    
- **Swagger / OpenAPI** → documentação automática da API.
    

---

### **Autenticação e Segurança**

- **Identity Server / ASP.NET Identity** → autenticação de usuários (ADM, empresa, turista).
    
- **JWT** → autenticação em APIs.
    
- **Role-based Access Control (RBAC)** → para diferenciar permissões.
    

---

### **Extras**

- **Serilog** → logs estruturados.
    
- **Health Checks** → monitorar status da API.
    
- **MailKit** → envio de e-mails (cadastro de empresa, reset de senha).
    
- **AbacatePay** → se quiser simular reservas/pagamentos [[10_Pagamentos]].
    
- **SignalR** → notificações em tempo real (ex: promoções instantâneas).