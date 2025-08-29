## **1️⃣ UX – Fluxo Geral**

### **Turista (PWA – Mobile)**

1. **Tela Inicial / Onboarding**
    
    - Botões: “Login”, “Cadastro”
        
    - Opção de idioma
        
    - Breve introdução: “Descubra empresas, serviços e eventos em Barra Bonita”
        
2. **Cadastro / Login**
    
    - Formulário com campos:
        
        - Nome, Email, Senha, Idioma
            
        - Botão “Cadastrar”
            
    - Login: Email + Senha
        
    - Recuperação de senha
        
3. **Tela Principal / Catálogo**
    
    - Lista de empresas e serviços
        
    - Filtros: Categoria, Preço, Localização
        
    - Botão de busca
        
    - Cards de serviço com: Nome, Foto, Preço, Distância, Avaliação média
        
4. **Detalhes do Serviço**
    
    - Foto, descrição detalhada
        
    - Botão: “Favoritar”
        
    - Botão: “Reservar / Pagar”
        
    - Avaliações e comentários
        
5. **Favoritos**
    
    - Lista de serviços/empresas favoritados
        
    - Botão para remover
        
    - Acesso rápido ao detalhe de cada serviço
        
6. **Reservas e Pagamentos**
    
    - Seleção de data/hora
        
    - Escolha do método de pagamento (AbacatePay / Pix / Cartão)
        
    - Confirmação e histórico
        
7. **Eventos / Promoções**
    
    - Lista de eventos ativos
        
    - Filtros por data ou categoria
        
    - Notificações de promoções
        
8. **Perfil**
    
    - Visualização e edição de dados
        
    - Histórico de reservas
        
    - Configuração de idioma, notificações
        

---

### **Empresa (Painel Desktop / Tablet)**

1. **Login**
    
    - Email + Senha
        
    - Redireciona para painel da empresa
        
2. **Dashboard**
    
    - Resumo de serviços cadastrados, reservas e pagamentos
        
    - Alertas de novas reservas ou mensagens
        
3. **Gerenciamento de Serviços / Eventos**
    
    - Lista de serviços/eventos
        
    - Botões: Adicionar, Editar, Remover
        
    - Upload de imagens e descrição
        
    - Status de visibilidade
        
4. **Reservas e Pagamentos**
    
    - Lista de reservas feitas
        
    - Status: pendente, confirmado, cancelado
        
    - Histórico de pagamentos com filtro por período
        
5. **Mensagens / Notificações**
    
    - Receber mensagens de turistas
        
    - Responder feedbacks
        
    - Notificações de novas reservas
        
6. **Analytics / Relatórios**
    
    - Métricas de acesso, reservas e engajamento
        
    - Gráficos: Favoritos, Visualizações, Pagamentos
        

---

### **Admin**

1. **Login Admin**
    
    - Email + Senha
        
    - Painel central de administração
        
2. **Dashboard**
    
    - Visualização geral: usuários, empresas, pagamentos
        
    - Alertas de cadastros pendentes
        
3. **Gerenciamento**
    
    - CRUD completo: Usuários, Empresas, Serviços, Eventos
        
    - Aprovação de novos cadastros
        
    - Logs de ações (audit trail)

---

### Wire Frames

```mermaid
flowchart TD
    %% ==========================
    %% TURISTA - PWA
    %% ==========================
    subgraph T_Turista [Turista - PWA]
        direction TB
        T1[Tela Inicial - Boas Vindas]
        T1 --> T2[Login]
        T1 --> T3[Cadastro]

        subgraph T_Catalogo [Catálogo de Serviços e Eventos]
            T4[Lista de Cards] --> T4a[Card: Imagem]
            T4 --> T4b[Card: Nome e Descrição]
            T4 --> T4c[Card: Preço ou Data]

            T5[Filtros e Busca] --> T5a[Categoria]
            T5 --> T5b[Pesquisa]
            T5 --> T5c[Ordenar por Distância ou Preço]
        end

        T2 --> T_Catalogo
        T3 --> T_Catalogo

        subgraph T_Detalhes [Detalhes do Serviço/Evento]
            T6[Descrição Completa]
            T6 --> T6a[Galeria de Imagens]
            T6 --> T6b[Avaliações]
            T6 --> T6c[Botões de Ação: Favoritar, Reservar, Pagar]
        end

        T_Catalogo --> T_Detalhes

        T6c --> T7[Pagamento AbacatePay, Pix ou Cartão] --> T8[Confirmação de Pagamento]
        T8 --> T9[Histórico de Reservas]

        T6 --> T10[Avaliações e Feedback] --> T10a[Escrever Avaliação] --> T10b[Ver Avaliações]
        T_Catalogo --> T11[Favoritos e Wishlist]
        T11 --> T6

        T12[Perfil do Turista] --> T_Catalogo
        T12 --> T11
        T12 --> T9
    end

    %% ==========================
    %% EMPRESA - Painel Desktop
    %% ==========================
    subgraph E_Empresa [Empresa - Painel]
        direction TB
        E1[Login Empresa] --> E2[Dashboard Empresa]

        subgraph E_Servicos [Gerenciar Serviços]
            E3[Lista de Serviços] --> E3a[Adicionar Serviço]
            E3 --> E3b[Editar Serviço]
            E3 --> E3c[Remover Serviço]
        end

        subgraph E_Eventos [Gerenciar Eventos e Promoções]
            E4[Lista de Eventos] --> E4a[Adicionar Evento]
            E4 --> E4b[Editar Evento]
            E4 --> E4c[Remover Evento]
        end

        subgraph E_Reservas [Reservas e Pagamentos]
            E5[Tabela de Reservas] --> E5a[Confirmar Reserva]
            E5 --> E5b[Cancelar Reserva]
            E5 --> E5c[Visualizar Pagamentos]
        end

        subgraph E_Analytics [Analytics e Relatórios]
            E6[Gráficos de Acesso e Engajamento]
        end

        subgraph E_Mensagens [Mensagens e Feedback]
            E7[Inbox] --> E7a[Responder Mensagens]
            E7 --> E7b[Responder Feedbacks]
        end

        E2 --> E_Servicos
        E2 --> E_Eventos
        E2 --> E_Reservas
        E2 --> E_Analytics
        E2 --> E_Mensagens
    end

    %% ==========================
    %% ADMIN - Painel
    %% ==========================
    subgraph A_Admin [Admin - Painel]
        direction TB
        A1[Login Admin] --> A2[Dashboard Admin]

        subgraph A_Usuarios [Gerenciar Usuários]
            A3[Lista de Usuários] --> A3a[Adicionar Usuário]
            A3 --> A3b[Editar Usuário]
            A3 --> A3c[Remover Usuário]
        end

        subgraph A_Empresas [Gerenciar Empresas]
            A4[Lista de Empresas] --> A4a[Aprovar Empresa]
            A4 --> A4b[Editar Empresa]
            A4 --> A4c[Remover Empresa]
        end

        subgraph A_ServicosEventos [Gerenciar Serviços e Eventos]
            A5[Lista de Serviços/Eventos] --> A5a[Editar Serviços]
            A5 --> A5b[Editar Eventos]
        end

        subgraph A_Pagamentos [Monitorar Pagamentos]
            A6[Tabela de Pagamentos e Histórico]
        end

        subgraph A_Logs [Logs e Auditoria]
            A7[Listagem de Ações do Sistema]
        end

        A2 --> A_Usuarios
        A2 --> A_Empresas
        A2 --> A_ServicosEventos
        A2 --> A_Pagamentos
        A2 --> A_Logs
    end

    %% ==========================
    %% Conexões Turista <-> Empresa/Admin
    %% ==========================
    T8 --> E5
    T10b --> E7
    T9 --> E5

    %% ==========================
    %% Estilo / Cores
    %% ==========================
    style T_Turista fill:#cce5ff,stroke:#3399ff,stroke-width:2px
    style T_Catalogo fill:#e6f2ff
    style T_Detalhes fill:#e6f2ff
    style E_Empresa fill:#d4edda,stroke:#28a745,stroke-width:2px
    style E_Servicos fill:#dff0d8
    style E_Eventos fill:#dff0d8
    style E_Reservas fill:#dff0d8
    style E_Analytics fill:#dff0d8
    style E_Mensagens fill:#dff0d8
    style A_Admin fill:#f8d7da,stroke:#dc3545,stroke-width:2px
    style A_Usuarios fill:#f5c6cb
    style A_Empresas fill:#f5c6cb
    style A_ServicosEventos fill:#f5c6cb
    style A_Pagamentos fill:#f5c6cb
    style A_Logs fill:#f5c6cb

```


---

```mermaid
```