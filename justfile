default:  
    echo "hello"

init-migration:
    dotnet dotnet-ef migrations add initialize -s ./src/SettlementService.Api/SettlementService.Api.csproj --project ./src/SettlementService.Infrastructure/SettlementService.Infrastructure.csproj -o ./Persistent/Migrations

database-update:
    dotnet dotnet-ef database update -s ./src/SettlementService.Api/SettlementService.Api.csproj --project ./src/SettlementService.Infrastructure/SettlementService.Infrastructure.csproj

