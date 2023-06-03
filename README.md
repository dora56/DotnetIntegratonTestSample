# DotNet Integration Test Sample
This sample demonstrates how to use the dotnet integration test

## Prerequisites
- [Docker](https://www.docker.com/products/docker-desktop)
- [Dotnet Core SDK](https://dotnet.microsoft.com/download)
- [EF Core CLI](https://docs.microsoft.com/ef/core/cli/dotnet)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli)
- [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local)
- [Azurite](https://learn.microsoft.comazure/storage/common/storage-use-azurite)
- [Azure Storage Explorer](https://azure.microsoft.com/features/storage-explorer/)
- [Sql Server Management Studio](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)

## Setup
1. Clone this repository
2. Open the solution in Editor of your choice
3. command line: `cd src/WebApp && dotnet ef database update`
4. FunctionApp Project add `local.settings.json` file with the following content:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  },
  "ConnectionStrings": {
    "DatabaseConnection": "Server=localhost;Database=functest;User=sa;Password=P@ssw0rd;Trusted_Connection=False;",
    "StorageConnection": "UseDevelopmentStorage=true"
  }
}
```
