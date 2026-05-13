# HexagFinance - Microserviço Financeiro Resiliente

Este projeto implementa um microserviço financeiro seguindo os princípios da **Arquitetura Hexagonal (Ports & Adapters)**, focado em alta disponibilidade, resiliência e consistência de dados.

## Tecnologias
- **Linguagem:** .NET 9 (C#)
- **Arquitetura:** Hexagonal (Ports & Adapters)
- **Banco de Dados:** MongoDB (NoSQL)
- **Mensageria:** RabbitMQ + MassTransit
- **Containerização:** Docker & Docker Compose
- **Logging:** Serilog (Estruturado)
- **Testes:** xUnit

## Arquitetura
O projeto está dividido em quatro camadas principais:
1.  **Domain:** Contém o coração do negócio (Entidades e Regras de Negócio). É uma camada pura, sem dependências de frameworks externos.
2.  **Application:** Orquestra o fluxo da aplicação, tratando idempotência e coordenando a comunicação entre Domínio e Infraestrutura.
3.  **Infrastructure:** Contém os adaptadores (MongoDB, RabbitMQ). Aqui a tecnologia é "plugada" no sistema.
4.  **API:** Porta de entrada REST que expõe os serviços para o mundo externo.

## Padrões de Resiliência e Consistência
- **Transactional Outbox:** Garante que mensagens nunca sejam perdidas. A transação financeira e a mensagem para a fila são salvas atomicamente no MongoDB.
- **Idempotência:** Proteção contra processamento duplicado de transações através de uma `IdempotencyKey` única no banco e na lógica de aplicação.
- **Global Exception Middleware:** Padronização de erros de negócio (400) e erros inesperados (500), mantendo os controladores limpos.
- **Injeção de Dependência Desacoplada:** Cada camada possui sua própria configuração de injeção, facilitando a manutenção.

## Como Executar
1.  Certifique-se de ter o **Docker Desktop** instalado.
2.  Na raiz do projeto, execute:
    ```bash
    docker-compose up -d
    ```
3.  Abra a solução no Visual Studio ou VS Code e execute o projeto `Financeiro.Api`.
4.  Acesse o Swagger em: `http://localhost:5000/swagger` (ou porta configurada).

## Testes
Para rodar os testes de unidade de domínio:
```bash
dotnet test
```

## Observabilidade
Os logs estruturados são gerados no console e também salvos em arquivos na pasta `/logs` do projeto API, permitindo rastrear transações por ID e Chave de Idempotência.
