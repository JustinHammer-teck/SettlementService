default:  
    echo "hello"

init-migrate:
    dotnet dotnet-ef migrations add initialize -s ./src/SettlementService.Api/SettlementService.Api.csproj --project ./src/SettlementService.Infrastructure/SettlementService.Infrastructure.csproj -o ./Persistent/Migrations

db-migrate MIGRATION_NAME:
    dotnet dotnet-ef migrations add {{MIGRATION_NAME}} -s ./src/SettlementService.Api/SettlementService.Api.csproj --project ./src/SettlementService.Infrastructure/SettlementService.Infrastructure.csproj -o ./Persistent/Migrations
    
db-update:
    dotnet dotnet-ef database update -s ./src/SettlementService.Api/SettlementService.Api.csproj --project ./src/SettlementService.Infrastructure/SettlementService.Infrastructure.csproj

