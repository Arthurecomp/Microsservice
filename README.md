# Desafio Técnico - Arquitetura de Microsserviços para E-commerce
- Este projeto implementa um sistema de back-end distribuído baseado na arquitetura de microsserviços, simulando as operações de estoque e vendas de um e-commerce. O sistema foi projetado com foco em desacoplamento, resiliência e segurança, utilizando comunicação síncrona e assíncrona para orquestrar as operações de negócio.

## Diagrama da Arquitetura
- O fluxo de comunicação do sistema segue o seguinte modelo:


``` plaintext
[Cliente] <--> [API Gateway] <--> [Microsserviços]
                            |
                            +--> [Estoque.API] <--> [Banco de Dados Estoque]
                            |
                            +--> [Vendas.API]  <--> [Banco de Dados Vendas]
                                     |
                                     +--(HTTP GET para validação)---> [Estoque.API]
                                     |
                                     +--(Publica Mensagem)-----------> [RabbitMQ]
                                                                           |
                                                                           +--(Consome Mensagem)--------> [Estoque.API]
```

## Tecnologias Utilizadas
- Back-end: .NET 8, C#, ASP.NET Core Web API

## Banco de Dados:
- SQL Server

## Acesso a Dados (ORM): 
- Entity Framework Core 8

## Comunicação Assíncrona:
- RabbitMQ com MassTransit

## API Gateway: 
- Ocelot

## Autenticação e Autorização: 
- JWT (JSON Web Tokens)


## Funcionalidades Principais
- ✅ Gerenciamento de Produtos: Endpoints RESTful completos (CRUD) para o gerenciamento de produtos no microsserviço de Estoque.

- ✅ Autenticação e Autorização: Sistema de login que gera um token JWT.

- ✅ Comunicação Síncrona: Antes de criar um pedido, o Vendas.API faz uma chamada HTTP direta ao Estoque.API para validar o estoque e obter o preço atual do produto, garantindo a consistência da transação.

- ✅ Comunicação Assíncrona: Após uma venda ser confirmada, o Vendas.API publica um evento no RabbitMQ. O Estoque.API consome este evento para dar baixa no estoque de forma desacoplada e resiliente.

- ✅ Ponto de Entrada Único: Um API Gateway configurado com Ocelot centraliza o acesso a todos os microsserviços, simplificando a interação do cliente.

- ✅ Ambiente Containerizado: Dockerizado o Banco de Dados e o Rabbit.

### Como Executar o Projeto
- Pré-requisitos:
  - Docker instalado e em execução.
  - .NET 8 SDK (opcional, para desenvolvimento local).

### Instruções:

- Clone este repositório.

- Abra um terminal na pasta raiz do projeto (onde o arquivo docker-compose.yml está localizado).

- Execute o comando para construir as imagens Docker dos serviços:
  - docker-compose build
  - Após o build ser concluído, inicie todos os contêineres:
  - docker-compose up -d
- Neste ponto, apenas o sqlserver e o rabbit está no Ar
  - RabbitMQ Management: http://localhost:15672 (Login: guest / guest)

- Rode os Microserviços em terminais separados, Rodar o de Estoque, Vendas e Gateway
  - Após isso pode usar a aplicação pela porta do API Gateway: http://localhost:5023

- Para parar e remover todos os contêineres, execute:

docker-compose down


## Endpoints da API (Via Gateway)
- Todas as requisições devem ser feitas para o API Gateway na porta 5023.

### Autenticação (Estoque.API)
```POST /api/estoque/Auth/register ```

### Registra um novo usuário.

- Autentica um usuário e retorna um token JWT.
  - Corpo: { "username": "user", "password": "password" }

 ```POST /api/estoque/Auth/login```


## Estoque (Estoque.API)
- Endpoints que requerem autenticação devem enviar o token no cabeçalho: Authorization: Bearer <seu_token>
  
- Lista todos os produtos. Requer autenticação.
```GET /api/estoque/Catalog ```
- Cria um novo produto.
```POST /api/estoque/Catalog```

- Atualiza um produto.
```PUT /api/estoque/Catalog/{id}```


## Vendas (Vendas.API)
- Cria um novo pedido.
Corpo: { "orderItems": [{ "productId": 1, "quantity": 2 }] }
```POST /api/vendas/Vendas ```


- Lista todos os pedidos com seus itens.
GET /api/vendas/Vendas
