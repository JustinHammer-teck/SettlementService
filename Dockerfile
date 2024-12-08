# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# Copy solution and project files
COPY ./src/SettlementService.Api/*.csproj ./src/SettlementService.Api/
COPY ./src/SettlementService.Application/*.csproj ./src/SettlementService.Application/
COPY ./src/SettlementService.Domain/*.csproj ./src/SettlementService.Domain/
COPY ./src/SettlementService.Infrastructure/.*csproj ./src/SettlementService.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY ./src ./src

# Build the application
RUN dotnet publish ./src/SettlementService.Api/SettlementService.Api.csproj -c Release -o /publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS runtime
WORKDIR /app

# Copy the build output
COPY --from=build /publish .

# Set environment variables
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:5000

# Expose the application port
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "SettlementService.Api.dll"]
