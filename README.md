# poc-usuario-identity

//dentro do projeto Data (Sempre apontar para um projeto que tenha a Connectionstring configurada no startup)
dotnet ef migrations add InitialMigration -s ../UserApi/UserApi.csproj


//pra atualizar o db na mão (Sempre apontar para um projeto que tenha a Connectionstring configurada no startup)
dotnet ef database update -s ../UserApi/UserApi.csproj 