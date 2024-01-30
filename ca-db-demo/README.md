CA DB Demo

## Tech Stack

- [x] Postgres SQL
- [x] CA
- [x]

````dotnetcli

# run db in docker
$ docker-compose up -d
$ docker ps
# stop db
$ docker-compose down

# create sln
$ dotnet new sln -o ca-db-demo

$ dotnet new webapi -o WebApi
$ dotnet new classlib -o Presentation
$ dotnet new classlib -o Infrastructure
$ dotnet new classlib -o Application
$ dotnet new classlib -o Domain

# powershell, add to sln
$ dotnet sln add (ls -r **/*.csproj)

$ dotnet add .\Domain\ package Newtonsoft.Json   

$ dotnet build

$ dotnet add .\Application\ package MediatR

$ dotnet add .\Application\ reference .\Domain\
$ dotnet add .\Infrastructure\ reference .\Application\
$ dotnet add .\Presentation\ reference .\Application\

```