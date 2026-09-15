# CardPay

CardPay é uma API REST desenvolvida em C# com .NET 10 para simular um fluxo simples de pagamentos com cartão.

A aplicação permite criar pagamentos, identificar automaticamente a bandeira do cartão, calcular parcelas, confirmar pagamentos pendentes e consultar pagamentos confirmados.

## Tecnologias

- C#
- .NET 10
- ASP.NET Core Web API
- Controllers
- OpenAPI
- Swagger
- ConcurrentDictionary

## Arquitetura

O projeto segue uma separação simples por responsabilidades:

- Controllers: recebem e respondem requisições HTTP.
- Services: concentram as regras de negócio.
- Repositories: armazenam os pagamentos em memória.
- Contracts: definem requests e responses da API.
- Domain: contém entidades, enums, constantes e exceptions.

Fluxo principal:

Controller → Service → Repository → Memória

## Funcionalidades

- Consulta de parcelamentos disponíveis.
- Consulta de bandeiras suportadas.
- Detecção automática da bandeira pelo número do cartão.
- Validação do cartão com algoritmo de Luhn.
- Criação de pagamentos pendentes.
- Cálculo automático das parcelas.
- Confirmação de pagamentos.
- Listagem de pagamentos confirmados.
- Armazenamento apenas em memória.

## Endpoints

| Método | Endpoint |
|---|---|
| GET | `/api/installments` |
| GET | `/api/card-brands` |
| POST | `/api/payments` |
| POST | `/api/payments/{id}/confirm` |
| GET | `/api/payments` |

## Executando o projeto

Pré-requisito:

- .NET 10 SDK

Restaurar dependências:

`dotnet restore`

Compilar:

`dotnet build`

Executar:

`dotnet run`

## Swagger

Com a aplicação em execução, a documentação da API pode ser acessada em:

`/swagger`

## Observações

Os pagamentos são armazenados somente em memória e são perdidos quando a aplicação é reiniciada.

O número completo do cartão não é armazenado após a identificação e validação da bandeira.