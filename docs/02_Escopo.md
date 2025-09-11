## **MVP 1 – Cadastro, Login e Perfis de Usuário**

- **Objetivo:** Permitir que turistas, empresas e administradores acessem a plataforma com base em seus perfis.
    
- **Funcionalidades:**
    
    - Cadastro de turista: nome, email, senha.
        
    - Cadastro de empresa: nome, email, senha, CNPJ/CPF, setor de atuação.
        
    - Cadastro de administrador pelo sistema (restrito).
        
    - Login com autenticação via **JWT**.
        
    - Recuperação de senha via email.
        
    - Perfis com **painéis diferenciados** (turista, empresa, admin).
        
- **Critério de sucesso:** Usuários conseguem se cadastrar, logar e acessar o painel correspondente com permissões distintas.
    

---

## **MVP 2 – Painel de Turista (PWA Mobile-First)**

- **Objetivo:** Prover experiência otimizada para turistas acessarem conteúdo.
    
- **Funcionalidades:**
    
    - Catálogo de empresas, serviços e eventos com busca e filtros.
        
    - Favoritar serviços/empresas/eventos.
        
    - Histórico de reservas.
        
    - Tradução básica para idioma preferido do usuário.
        
    - Notificações (push ou em tela) para atualizações de reservas e promoções.
        
- **Critério de sucesso:** Turista consegue **buscar, visualizar e favoritar** serviços/eventos e acessar histórico de reservas.
    

---

## **MVP 3 – Painel de Empresa (Gestão de Serviços e Eventos)**

- **Objetivo:** Permitir que empresas cadastrem e acompanhem engajamento.
    
- **Funcionalidades:**
    
    - Cadastro de serviços, produtos e eventos (com título, descrição, preço, imagens).
        
    - Edição/remoção de serviços/eventos.
        
    - Visualização de métricas: acessos, reservas e favoritos.
        
    - Recebimento de notificações de novas reservas.
        
- **Critério de sucesso:** Empresa consegue **cadastrar serviços/eventos, acompanhar engajamento** e receber notificações de novas reservas.
    

---

## **MVP 4 – Reservas e Pagamentos (AbacatePay)**

- **Objetivo:** Permitir que turistas realizem reservas e paguem dentro da plataforma.
    
- **Funcionalidades:**
    
    - Fluxo de reserva: turista seleciona serviço → empresa recebe notificação.
        
    - Integração com **AbacatePay** para checkout seguro.
        
    - Webhook para confirmação automática de pagamento.
        
    - Atualização automática do status da reserva (pendente → pago → confirmado).
        
    - Histórico de transações para empresa e turista.
        
- **Critério de sucesso:** Turista consegue **reservar e pagar** por serviços, empresa recebe confirmação no painel e status é atualizado automaticamente.
    

---

## **MVP 5 – Administração e Monitoramento**

- **Objetivo:** Garantir governança e segurança na plataforma.
    
- **Funcionalidades:**
    
    - Painel de administração (CRUD de usuários, empresas e eventos).
        
    - Aprovação de empresas antes de ficarem visíveis no catálogo.
        
    - Monitoramento de logs e métricas do sistema.
        
    - Visualização de pagamentos e reservas em tempo real.
        
    - Possibilidade de bloquear ou reativar usuários/empresas.
        
- **Critério de sucesso:** Admin consegue **aprovar cadastros, acompanhar pagamentos e gerenciar o ecossistema** com segurança.
    

---

## **MVP 6 – Mensageria e Feedback** _(incremento opcional após MVP 5)_

- **Objetivo:** Melhorar a interação entre turistas e empresas.
    
- **Funcionalidades:**
    
    - Chat em tempo real via **SignalR**.
        
    - Feedback de turistas (avaliação e comentário sobre serviços e eventos).
        
    - Relatórios de satisfação para empresas.
        
- **Critério de sucesso:** Turista consegue **avaliar serviços** e empresas conseguem **visualizar feedbacks** e responder mensagens.

---
