# HexagFinance - Microserviço Financeiro Resiliente (V2)

Este projeto evoluiu de um microserviço puramente backend para uma solução **Full-Stack (V2)**, integrando um frontend moderno em Angular e reforçando a resiliência do backend com padrões avançados de mensageria e persistência.

O sistema segue os princípios da **Arquitetura Hexagonal (Ports & Adapters)**, focado em alta disponibilidade, resiliência e consistência de dados.

## 🔄 Evolução: V1 vs V2

| Recurso | Versão 1.0 (Backend Only) | Versão 2.0 (Full-Stack) |
| :--- | :--- | :--- |
| **Interface** | Apenas Swagger / Postman | Frontend Angular 18+ (Signals & Dark Mode) |
| **Operações** | Apenas Envio de Transações (POST) | CRUD Completo com Listagem (GET) |
| **Consistência** | Persistência Simples | **Transactional Outbox** com MongoDB Replica Set |
| **Integração** | Sem suporte a CORS | CORS configurado para Web Apps |
| **Infra** | MongoDB Standalone | MongoDB Replica Set (Suporte a Transações) |
| **Resiliência** | Básica | Retry Policies, Idempotência e Outbox pattern |

## 🚀 Novidades da V2
- **Frontend Angular:** Interface moderna em Angular 18+ com Signals, Dark Mode e suporte Multi-idioma (i18n).
- **Full CRUD Support:** Adição do endpoint de listagem (GET) para visualização em tempo real das transações no Dashboard.
- **Replica Set Local:** Configuração do MongoDB em modo Replica Set para suporte nativo a transações distribuídas via Transactional Outbox do MassTransit.
- **CORS Configuration:** Backend preparado para comunicação segura e fluida com o frontend.

## 🛠️ Tecnologias
- **Backend:** .NET 9 (C#)
- **Frontend:** Angular 18 (Signals, SCSS, i18n)
- **Arquitetura:** Hexagonal (Ports & Adapters)
- **Banco de Dados:** MongoDB (Replica Set ativado)
- **Mensageria:** RabbitMQ + MassTransit (v8.2.5 para compatibilidade)
- **Containerização:** Docker & Docker Compose
- **Logging:** Serilog (Estruturado)
- **Testes:** xUnit

## 🏗️ Arquitetura
O projeto está dividido em camadas desacopladas:
1.  **Domain:** Core do negócio (Entidades e Regras). Camada pura e sem dependências.
2.  **Application:** Orquestra fluxos, trata idempotência e coordena comunicação.
3.  **Infrastructure:** Adaptadores para persistência (MongoDB) e mensageria (RabbitMQ).
4.  **API:** Porta de entrada REST com Swagger e CORS configurado.

## 💎 Resiliência e Consistência
- **Transactional Outbox:** Garante entrega de mensagens (At-least-once delivery) integrando a transação do banco com o envio para a fila de forma atômica.
- **Idempotência:** Proteção contra duplicidade via `IdempotencyKey`.
- **Global Exception Handling:** Middleware centralizado para tratamento de erros padronizados.

## 🚦 Como Executar

### 1. Infraestrutura (Docker)
Na raiz do projeto backend, inicie os containers:
```bash
docker-compose up -d
```
*Nota: O MongoDB iniciará como um Single Node Replica Set para permitir transações.*

### 2. Backend (.NET)
Abra o projeto e execute:
```bash
dotnet run --project Financeiro.Api
```
A API estará disponível em: `http://localhost:5201/swagger`

### 3. Frontend (Angular)
No diretório do frontend (`HexagFinanceFront`), execute:
```bash
npm install
npm start
```
Acesse em: `http://localhost:4200`

## 🧪 Testes
Para rodar os testes de unidade:
```bash
dotnet test
```

## 📊 Observabilidade
Logs estruturados são gerados via Serilog e podem ser encontrados na pasta `/logs` da API, permitindo o rastreamento completo de cada transação financeira.
