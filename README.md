# SendEmail API

## Visão Geral

SendEmail é uma API desenvolvida para gerenciar autenticação de usuários, envio de emails via SendGrid e upload de imagens para um banco de dados. Facilita login seguro, autenticação, envio de emails e upload de imagens.

## Funcionalidades

### Autenticação de Usuários
- Login e autenticação seguros.

### Envio de Emails
- Utiliza SendGrid para enviar emails.

### Upload de Imagens
- Upload e gerenciamento de imagens no banco de dados.

## Tecnologias Utilizadas

- **C#**: Linguagem de programação principal.
- **SendGrid**: Serviço de email.
- **Entity Framework**: Para interações com o banco de dados.
- **ASP.NET Core**: Framework para API web.

## Como Começar

### Pré-requisitos

-.NET SDK
- Chave de API SendGrid
- Banco de dados (por exemplo, SQL Server)

### Instalação

1. Clone o repositório:
bash git clone https://github.com/BrunoSeraguza/SendEmail.git

2. Navegue até o diretório do projeto:
bash cd SendEmail

3. Configure a conexão com o banco de dados e a chave da API SendGrid em `appsettings.json`.
4. Restaure as dependências e construa o projeto:
bash dotnet restore dotnet build


### Executando a Aplicação

Execute a aplicação:
bash dotnet run


## Endpoints da API

- `/v1/accounts/login`: Autenticação de usuários.
- `/v1/accounts`: Envio de email.
- `/v1/accounts/UploadImage`: Upload de imagem.

## Contribuição

Contribuições são bem-vindas Faça um fork do repositório e envie um pull reque
