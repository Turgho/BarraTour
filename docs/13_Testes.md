### 🎯 Objetivo

Testar funções e métodos isolados, sem dependências externas.

---

| Módulo                     | Tipo de Teste | Cenário                                 | Resultado Esperado                                        |
| -------------------------- | ------------- | --------------------------------------- | --------------------------------------------------------- |
| **Users / Autenticação**   | Unitário      | Criar usuário com email e senha         | Usuário criado, senha armazenada com hash, role atribuída |
|                            | Unitário      | Login com senha correta                 | Retorna JWT válido e refresh token                        |
|                            | Unitário      | Login com senha incorreta               | Retorna erro, incrementa FailedLoginAttempts              |
|                            | Integração    | Login + criar reserva                   | JWT válido permite criar reserva, reserva salva no DB     |
|                            | Segurança     | Token expirado                          | Acesso negado, refresh token pode renovar                 |
| **Bookings / Reservas**    | Unitário      | Criar reserva                           | Reserva criada com status `pending`                       |
|                            | Unitário      | Cancelar reserva                        | Reserva status = `cancelled`                              |
|                            | Integração    | Reserva → pagamento → webhook pago      | Status reserva atualizado para `confirmed`                |
|                            | E2E           | Turista realiza reserva completa        | Reserva criada, pagamento aprovado, notificação enviada   |
| **Payments / Pagamentos**  | Unitário      | Criar ordem de pagamento                | Order criado com status `pending` e valor correto         |
|                            | Unitário      | Receber webhook `paid`                  | Atualiza status do pagamento e reserva                    |
|                            | Integração    | Pagamento falho                         | Status `failed`, notificação de erro enviada              |
|                            | E2E           | Checkout completo                       | Pagamento refletido na UI, reserva confirmada             |
|                            | Segurança     | Webhook falso                           | Transação rejeitada, status não alterado                  |
| **Companies / Serviços**   | Unitário      | Criar serviço                           | Serviço salvo no DB, vinculado à empresa                  |
|                            | Integração    | Criar serviço → listar catálogo         | Serviço aparece corretamente no catálogo do turista       |
|                            | E2E           | Empresa adiciona evento/promocao        | Evento aparece no painel e para turista                   |
| **Mensageria**             | Unitário      | Enviar mensagem                         | Mensagem salva no DB, IsRead = false                      |
|                            | Integração    | Turista envia → Empresa responde        | Mensagem chega em tempo real via WebSocket/Push           |
|                            | E2E           | Chat completo                           | Mensagens visíveis para ambos, notificações corretas      |
|                            | Segurança     | Usuario tenta acessar mensagem de outro | Acesso negado, mensagem não exibida                       |
| **Feedback / Avaliações**  | Unitário      | Criar feedback                          | Avaliação salva com rating correto e comentário           |
|                            | Integração    | Feedback vinculado à reserva            | Feedback associado ao serviço/empresa corretamente        |
|                            | E2E           | Turista avalia serviço                  | Avaliação aparece no painel da empresa                    |
| **Segurança Geral**        | Unitário      | Roles restritas                         | Turista não acessa painel Empresa/Admin                   |
|                            | Integração    | Usuario sem token acessa rota           | API retorna 401 Unauthorized                              |
|                            | E2E           | Tentativa de SQL Injection              | Ataque rejeitado, dados protegidos                        |
| **Performance (opcional)** | Stress        | 50 turistas simultâneos                 | Sistema mantém tempo de resposta aceitável                |
|                            | Stress        | Mensageria em tempo real                | Mensagens entregues em <1s                                |