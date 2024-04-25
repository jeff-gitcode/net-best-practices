# SAGA Demo

## Tech Stack
-[x] 
 
```dotnetcli

#create sln
$ dotnet new sln -o saga-demo

# cd saga-demo
$ dotnet new webapi -o WebApi
$ dotnet new classlib -o Presentation
$ dotnet new classlib -o Infrastructure
$ dotnet new classlib -o Application
$ dotnet new classlib -o Domain

# NServiceBus
$ dotnet add .\webapi\ package NServiceBus

# powershell, add to sln
$ dotnet sln add (ls -r **/*.csproj)
```