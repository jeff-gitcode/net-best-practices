Docker Demo

```dotnetcli


$ dotnet new install Clean.Lean.Architecture.WebApi.AspNetCore.Solution.Template
$ dotnet new cla-sln --name DockerWebApiDemo
$ cd .\DockerWebApiDemo\src\WebApi\
$ dotnet run --project .\DockerWebApiDemo\src\WebApi\
# cd .\DockerWebApiDemo
$ docker init
$ docker compose up --build
# http://localhost:8080
$ docker compose down -v
$ dotnet workload update
$ dotnet tool install -g aspirate
$ aspirate
$ aspirate init
# cd ./src/DockerWebApiDemo.AppHost
$ aspirate generate -- output-format compose

```
