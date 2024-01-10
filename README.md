# FilmeAPI - Web API de Cadastro de Filmes

Esta é uma Web API simples desenvolvida em ASP.NET Core para gerenciar um cadastro de filmes em um banco de dados MySQL.

## Descrição

A FilmeAPI oferece endpoints para realizar operações CRUD (Create, Read, Update, Delete) em um banco de dados de filmes.

## Funcionalidades

- Adiciona filmes ao banco de dados.
- Recupera informações dos filmes armazenados.
- Atualiza informações de filmes.
- Remove filmes do banco de dados.

## Tecnologias Utilizadas

- ASP.NET Core Web API (versão .NET 6.0) para o desenvolvimento do back-end
- Entity Framework Core (versão 7.0)
- MySQL
- AutoMapper

### Configuração do Entity Framework Core

Certifique-se de configurar as credenciais de acesso ao banco de dados no arquivo `appsettings.json`. As migrações do Entity Framework Core devem ser aplicadas para criar as tabelas necessárias no banco de dados.

## Endpoints da API

### POST /api/Filme

Adiciona um novo filme ao banco de dados.

### GET /api/Filme

Recupera uma lista de filmes armazenados.

### GET /api/Filme/{id}

Recupera informações de um filme específico com base no seu ID.

### PUT /api/Filme/{id}

Atualiza as informações de um filme existente.

### DELETE /api/Filme/{id}

Remove um filme do banco de dados com base no seu ID.

## Pré-requisitos

- [.NET Core SDK](https://dotnet.microsoft.com/download) instalado.
- [Visual Studio](https://visualstudio.microsoft.com/) ou [Visual Studio Code](https://code.visualstudio.com/) para desenvolvimento (opcional).
- Banco de dados MySql ou outro banco de dados compatível.


## Instalação e Execução

1. Clone o repositório: `git clone https://github.com/Joaovittorsd/FilmesApi.git`
2. Configure as credenciais do banco de dados no arquivo `appsettings.json`.
3. Instale as dependências do projeto: `dotnet restore`
4. Execute as migrações para criar o banco de dados.
gsoftp@gsoservico.com.br
Gsoservico@ftp
