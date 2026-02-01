FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY HttpRequestPipeline.Gateway/HttpRequestPipeline.Gateway.csproj HttpRequestPipeline.Gateway/
RUN dotnet restore HttpRequestPipeline.Gateway/HttpRequestPipeline.Gateway.csproj

COPY . .
RUN dotnet publish HttpRequestPipeline.Gateway/HttpRequestPipeline.Gateway.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Health check için curl kur
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 5294
ENTRYPOINT ["dotnet", "HttpRequestPipeline.Gateway.dll"]