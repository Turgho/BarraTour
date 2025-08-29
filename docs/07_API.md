 Todos os endpoints devem retornar **um objeto JSON com a chave `data`** e um **status HTTP apropriado**

| Campo   | Tipo    | Descrição                                                           |
| ------- | ------- | ------------------------------------------------------------------- |
| data    | object  | Contém o resultado da operação (objeto, lista ou vazio).            |
| message | string  | Mensagem resumida sobre o resultado (sucesso ou erro).              |
| success | boolean | Indica se a operação foi bem-sucedida (`true`) ou falhou (`false`). |
| errors  | array   | Lista de erros detalhados caso a operação falhe.                    |
#### 🔹 Campos do Retorno - Exemplo

```json
{
  "data": {},
  "message": "Operação realizada com sucesso",
  "success": true,
  "errors": []
}
```

#### 1️⃣ **Sucesso – Consulta de usuário**

```json
{
  "data": {
    "userId": "d7a1b6c4-5e2f-4f3d-8b2e-3f5d4c1a2b7c",
    "name": "Victor Hugo",
    "email": "victor@email.com",
    "role": "tourist",
    "status": true
  },
  "message": "Usuário encontrado com sucesso",
  "success": true,
  "errors": []
}
```

#### **2️⃣ Sucesso – Lista de serviços**

```json
{
  "data": [
    {
      "serviceId": "a1b2c3d4-5e6f-7g8h-9i0j-1234567890ab",
      "title": "Passeio de barco",
      "category": "natureza",
      "price": 120.50
    },
    {
      "serviceId": "b2c3d4e5-6f7g-8h9i-0j1k-234567890abc",
      "title": "Museu Histórico",
      "category": "histórico",
      "price": 30.00
    }
  ],
  "message": "Serviços carregados com sucesso",
  "success": true,
  "errors": []
}
```

#### **3️⃣ Erro – Usuário não encontrado**

```json
{
  "data": {},
  "message": "Usuário não encontrado",
  "success": false,
  "errors": ["ID de usuário inválido ou inexistente"]
}
```

#### 🔹 Padrão de **HTTP Status Code**

|Código|Uso|
|---|---|
|200|Sucesso (GET, PUT, PATCH)|
|201|Recurso criado com sucesso (POST)|
|204|Requisição bem-sucedida sem conteúdo|
|400|Erro de validação / Requisição inválida|
|401|Não autorizado / Falha de autenticação|
|403|Proibido / Usuário sem permissão|
|404|Recurso não encontrado|
|409|Conflito / Recurso já existe|
|500|Erro interno do servidor|

### 💡 **Observações:**

- O campo `data` sempre existe, mesmo que vazio (`{}`) em caso de erro.
    
- `message` deve ser **legível e curto**, útil para frontend mostrar notificações.
    
- `errors` deve conter **detalhes técnicos** para debug (opcional para o usuário final).

### 📦 Tipos de Retorno da API – Campo `data`

| User Story / Endpoint           | Tipo de Retorno | Estrutura / Conteúdo                                                          | Observações                                     |
| ------------------------------- | --------------- | ----------------------------------------------------------------------------- | ----------------------------------------------- |
| US01 – Cadastro/Login           | Objeto          | `{ userId, name, email, role, status }`                                       | Retorna dados do usuário recém-criado ou logado |
| US02 – Cadastro/Login Empresa   | Objeto          | `{ companyId, userId, name, address, email, phone }`                          | Retorna dados da empresa vinculada ao usuário   |
| US03 – Listar Serviços          | Lista           | `[ { serviceId, title, category, price } ]`                                   | Pode incluir filtros e ordenação                |
| US04 – Favoritar Serviço        | Objeto          | `{ favoriteId, userId, serviceId, createdAt }`                                | Confirmação de adição/removido dos favoritos    |
| US05 – Listar Eventos           | Lista / Objeto  | `{ items: [...], totalItems, currentPage, totalPages }`                       | Permite paginação e metadata                    |
| US06 – Cadastrar Serviços       | Objeto          | `{ serviceId, companyId, title, description, price }`                         | Confirmação de criação do serviço               |
| US07 – Consultar Reservas       | Lista           | `[ { bookingId, userId, serviceId, status, bookingDate } ]`                   | Para painel de empresas ou usuário              |
| US08 – Criar Pagamento          | Objeto          | `{ paymentId, bookingId, amount, method, status, createdAt }`                 | Retorna status da transação                     |
| US09 – Webhook Pagamento        | Objeto          | `{ bookingId, paymentId, status, updatedAt }`                                 | Atualiza status da reserva/pagamento            |
| US10 – Administração            | Lista / Objeto  | `[ { userId, name, email, role, status } ]` ou `{ companyId, name, address }` | Retorna recursos administráveis                 |
| US11 – Feedback                 | Objeto          | `{ feedbackId, userId, serviceId, companyId, rating, comment, createdAt }`    | Confirmação de feedback enviado                 |
| US12 – Relatórios               | Objeto / Lista  | `{ totalBookings, totalRevenue, averageRating, bookingsByCategory }`          | Dashboard de métricas                           |
| US13 – Favoritos / Wishlist     | Lista           | `[ { favoriteId, serviceId, companyId, createdAt } ]`                         | Lista de favoritos do usuário                   |
| US14 – Eventos / Promoções      | Lista           | `[ { eventId, companyId, title, description, startDate, endDate } ]`          | Exibe eventos e promoções                       |
| US15 – Editar Perfil Usuário    | Objeto          | `{ userId, name, email, phone, status }`                                      | Retorno atualizado do perfil                    |
| US16 – Editar Perfil Empresa    | Objeto          | `{ companyId, name, address, phone, email }`                                  | Retorno atualizado da empresa                   |
| US17 – Histórico de Reservas    | Lista           | `[ { bookingId, serviceId, status, bookingDate } ]`                           | Histórico completo de reservas do usuário       |
| US18 – Busca/Filtragem Serviços | Lista / Objeto  | `[ { serviceId, title, category, price } ]` ou `{ items: [...], totalItems }` | Paginação e filtros aplicáveis                  |
| US19 – Geolocalização           | Lista           | `[ { serviceId, title, latitude, longitude, distance } ]`                     | Lista de serviços próximos ao usuário           |
| US20 – Notificações             | Lista           | `[ { notificationId, title, message, createdAt, read } ]`                     | Notificações ativas para o usuário              |
| US21 – Mensagens                | Lista / Objeto  | `[ { messageId, userId, companyId, content, createdAt } ]`                    | Histórico ou envio de mensagens                 |
| US22 – Responder Feedback       | Objeto          | `{ feedbackId, response, respondedAt }`                                       | Retorno de resposta ao feedback                 |
| US23 – Painel Analytics         | Objeto          | `{ totalBookings, totalRevenue, topServices, engagementMetrics }`             | Métricas e gráficos para empresas               |
| US24 – Criar Coupon             | Objeto          | `{ couponId, code, discount, serviceId, validUntil, usageLimit }`             | Retorno do cupom criado                         |