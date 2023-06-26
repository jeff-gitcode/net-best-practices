# .Net Best Practices

# Tech Stack

- [x] Json source generator
- [x] Minimal API
- [x] Coravel
- [x] RestSharp
-
-

![alt text](./doc/coravel.jpg)

```c#
# json source generator
$ dotnet new sln
$ dotnet new web -o json-source-generator
$ dotnet sln add json-source-generator

$ dotnet add .\json-source-generator\ package Swashbuckle.AspNetCore

$ dotnet dev-certs https --trust

$ dotnet build

$ dotnet run --project .\json-source-generator\json-source-generator.csproj

# cravel
$ dotnet new webapi -o coravel-demo
$ dotnet add package coravel

# restsharp
$ dotnet new webapi -o restsharp-demo
$ dotnet sln add restsharp-demo
$ dotnet add .\restsharp-demo\ package RestSharp
$ dotnet add .\restsharp-demo\ package Newtonsoft.Json
```
