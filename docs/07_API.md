# Endpoints da API - Estrutura Padronizada

Com base nos 12 US revisados e no formato de resposta padrão, aqui está a estrutura dos endpoints:

```json
{
  "data": {},
  "message": "Operação realizada com sucesso",
  "success": true,
  "errors": []
}
```

## 📋 Endpoints por User Story

### US01 - Autenticação de Usuários

```http
POST `/api/auth/register`
```

```json
{
  "data": {
    "userId": "guid",
    "name": "string",
    "email": "string",
    "role": "tourist",
    "status": "active",
    "emailVerified": false
  },
  "message": "Usuário criado com sucesso",
  "success": true,
  "errors": []
}
```

```http
POST `/api/auth/login`
```

```json
{
  "data": {
    "userId": "guid",
    "name": "string",
    "email": "string",
    "role": "tourist",
    "token": "jwt-token",
    "refreshToken": "refresh-token"
  },
  "message": "Login realizado com sucesso",
  "success": true,
  "errors": []
}
```

### US02 - Cadastro de Empresas

```http
POST `/api/auth/register-company`
```

```json
{
  "data": {
    "companyId": "guid",
    "userId": "guid",
    "name": "string",
    "cnpj": "string",
    "address": "string",
    "approvalStatus": "pending"
  },
  "message": "Empresa cadastrada com sucesso - Aguardando aprovação",
  "success": true,
  "errors": []
}
```

### US03 - Gestão de Perfis

```http
PUT `/api/user/profile`
```

```json
{
  "data": {
    "userId": "guid",
    "name": "string",
    "email": "string",
    "phone": "string",
    "preferences": {}
  },
  "message": "Perfil atualizado com sucesso",
  "success": true,
  "errors": []
}
```

### US04 - Busca de Serviços

```http
GET `/api/services?category=tours&priceRange=50-200&location=-23.5505,-46.6333`
```

```json
{
  "data": {
    "items": [
      {
        "serviceId": "guid",
        "title": "string",
        "category": "string",
        "price": 100.50,
        "rating": 4.5,
        "companyName": "string",
        "distance": 2.5
      }
    ],
    "totalItems": 25,
    "currentPage": 1,
    "totalPages": 3,
    "filters": {
      "category": "tours",
      "priceRange": "50-200"
    }
  },
  "message": "Serviços encontrados",
  "success": true,
  "errors": []
}
```

### US05 - Detalhes de Serviços

```http
GET `/api/services/{id}`
```

```json
{
  "data": {
    "serviceId": "guid",
    "title": "string",
    "description": "string",
    "category": "string",
    "price": 100.50,
    "duration": 120,
    "maxParticipants": 10,
    "company": {
      "companyId": "guid",
      "name": "string",
      "rating": 4.7
    },
    "images": ["url1", "url2"],
    "requirements": "string",
    "meetingPoint": "string",
    "availability": [
      {
        "date": "2025-01-15",
        "availableSlots": 5
      }
    ]
  },
  "message": "Detalhes do serviço",
  "success": true,
  "errors": []
}
```

### US06 - Cadastro de Serviços

```http
POST `/api/company/services`
```

```json
{
  "data": {
    "serviceId": "guid",
    "title": "string",
    "description": "string",
    "category": "string",
    "price": 100.50,
    "duration": 120,
    "maxParticipants": 10,
    "createdAt": "2025-01-10T10:00:00Z"
  },
  "message": "Serviço criado com sucesso",
  "success": true,
  "errors": []
}
```

### US07 - Criação de Reservas

```http
POST `/api/bookings`
```

```json
{
  "data": {
    "bookingId": "guid",
    "serviceId": "guid",
    "userId": "guid",
    "status": "pending",
    "bookingDate": "2025-01-15T14:00:00Z",
    "participants": 2,
    "totalAmount": 201.00,
    "createdAt": "2025-01-10T10:00:00Z"
  },
  "message": "Reserva criada com sucesso",
  "success": true,
  "errors": []
}
```

### US08 - Processamento de Pagamentos

```http
POST `/api/payments`
```

```json
{
  "data": {
    "paymentId": "guid",
    "bookingId": "guid",
    "amount": 201.00,
    "method": "abacate_pay",
    "status": "pending",
    "transactionId": "string",
    "createdAt": "2025-01-10T10:00:00Z"
  },
  "message": "Pagamento iniciado com sucesso",
  "success": true,
  "errors": []
}
```

### US09 - Gestão de Reservas

```http
GET `/api/company/bookings`
```

```json
{
  "data": {
    "items": [
      {
        "bookingId": "guid",
        "serviceName": "string",
        "customerName": "string",
        "status": "confirmed",
        "bookingDate": "2025-01-15T14:00:00Z",
        "participants": 2,
        "totalAmount": 201.00
      }
    ],
    "totalItems": 15,
    "currentPage": 1,
    "totalPages": 2
  },
  "message": "Reservas da empresa",
  "success": true,
  "errors": []
}
```

### US10 - Administração de Empresas

```http
GET `/api/admin/companies?status=pending`
```

```json
{
  "data": {
    "items": [
      {
        "companyId": "guid",
        "name": "string",
        "cnpj": "string",
        "email": "string",
        "approvalStatus": "pending",
        "createdAt": "2025-01-05T10:00:00Z"
      }
    ],
    "totalItems": 8,
    "currentPage": 1,
    "totalPages": 1
  },
  "message": "Empresas pendentes de aprovação",
  "success": true,
  "errors": []
}
```

```http
PUT `/api/admin/companies/{id}/approve`
```

```json
{
  "data": {
    "companyId": "guid",
    "name": "string",
    "approvalStatus": "approved",
    "approvedAt": "2025-01-10T10:00:00Z"
  },
  "message": "Empresa aprovada com sucesso",
  "success": true,
  "errors": []
}
```
### US11 - Sistema de Favoritos

```http
POST `/api/favorites`
```

```json
{
  "data": {
    "favoriteId": "guid",
    "userId": "guid",
    "serviceId": "guid",
    "createdAt": "2025-01-10T10:00:00Z"
  },
  "message": "Serviço adicionado aos favoritos",
  "success": true,
  "errors": []
}
```

**GET** `/api/favorites`

```json
{
  "data": {
    "items": [
      {
        "favoriteId": "guid",
        "serviceId": "guid",
        "serviceName": "string",
        "servicePrice": 100.50,
        "companyName": "string",
        "addedAt": "2025-01-10T10:00:00Z"
      }
    ],
    "totalItems": 5
  },
  "message": "Lista de favoritos",
  "success": true,
  "errors": []
}
```

### US12 - Sistema de Avaliações

```http
POST `/api/reviews`
```

```json
{
  "data": {
    "reviewId": "guid",
    "bookingId": "guid",
    "serviceId": "guid",
    "userId": "guid",
    "rating": 5,
    "comment": "string",
    "createdAt": "2025-01-10T10:00:00Z"
  },
  "message": "Avaliação enviada com sucesso",
  "success": true,
  "errors": []
}
```
### US13 - Sistema de Notificações

```http
GET `/api/notifications`
```

```json
{
  "data": {
    "items": [
      {
        "notificationId": "guid",
        "title": "string",
        "message": "string",
        "type": "info",
        "isRead": false,
        "createdAt": "2025-01-10T10:00:00Z"
      }
    ],
    "unreadCount": 3
  },
  "message": "Notificações do usuário",
  "success": true,
  "errors": []
}
```

### US14 - Relatórios e Analytics

```http
GET `/api/company/analytics?startDate=2025-01-01&endDate=2025-01-31`
```

```json
{
  "data": {
    "period": {
      "startDate": "2025-01-01",
      "endDate": "2025-01-31"
    },
    "metrics": {
      "totalBookings": 45,
      "totalRevenue": 4525.75,
      "averageRating": 4.7,
      "cancellationRate": 0.05
    },
    "bookingsByCategory": [
      {
        "category": "tours",
        "count": 25,
        "revenue": 2500.00
      }
    ],
    "topServices": [
      {
        "serviceId": "guid",
        "name": "string",
        "bookings": 15,
        "revenue": 1500.00
      }
    ]
  },
  "message": "Relatório gerado com sucesso",
  "success": true,
  "errors": []
}
```

## 📊 Paginação e Filtros

Todos os endpoints que retornam listas suportam paginação:

```http
GET /api/resources?page=1&pageSize=20&sortBy=name&sortOrder=asc
```

## 🎯 Tratamento de Erros

Exemplo de resposta de erro:

```json
{
  "data": null,
  "message": "Falha na operação",
  "success": false,
  "errors": [
    {
      "code": "VALIDATION_ERROR",
      "message": "Email já cadastrado",
      "field": "email"
    }
  ]
}
```

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
