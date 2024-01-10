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

- Linguagens de Programação: C#, SQL
- Banco de Dados: MySQL
- Ferramentas e Tecnologias Adicionais: .NET Core SDK, Visual Studio (ou Visual Studio Code), Git, Postman

### Frameworks e Bibliotecas

- **ASP.NET Core (versão 6.5.0):** Utilizado como base para o desenvolvimento da Web API.
- **Entity Framework Core (versão 7.0.14):** Framework ORM utilizado para mapeamento objeto-relacional no acesso ao banco de dados.
- **AutoMapper:** Utilizado para mapeamento de objetos.
- **Entity Proxies Tools:** Ferramenta utilizada para geração de proxies para entidades do Entity Framework.
- **Newtonsoft.Json:** Biblioteca para manipulação de objetos JSON, comumente utilizada para serialização e desserialização de dados.


### Configuração do Entity Framework Core

Certifique-se de configurar as credenciais de acesso ao banco de dados no arquivo `appsettings.json`. As migrações do Entity Framework Core devem ser aplicadas para criar as tabelas necessárias no banco de dados.

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
