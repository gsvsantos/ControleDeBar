# Controle de Bar

Uma simples aplicação web para gestão de um bar, com funcionalidades como controle de pedidos, mesas, garçons e produtos.

## Funcionalidades

- Cadastro de Mesas
- Cadastro de Garçons
- Cadastro de Produtos
- Registro de Pedidos por Mesa
- Fechamento e Visualização de Contas

## Como executar

1. Instale o [.NET 8 SDK](https://dotnet.microsoft.com/download).
2. Abra a solução `ControleDeBar.sln` no Visual Studio 2022 ou superior.
3. Defina o projeto `ControleDeBar.WebApp` como **Startup Project**.
4. Execute a aplicação (`F5` ou `Ctrl + F5`).

## Projetos da Solução

- **ControleDeBar.WebApp** – Interface web (ASP.NET Core)
- **ControleDeBar.Dominio** – Entidades e regras de negócio
- **ControleDeBar.Infraestrutura.SQLServer** – Persistência em SQL Server

## Requisitos

- .NET 8 SDK
- Visual Studio 2022+
- SQL Server (caso use a persistência por banco de dados)

---


_readme em desenvolvimento, mais detalhes futuramente!_
