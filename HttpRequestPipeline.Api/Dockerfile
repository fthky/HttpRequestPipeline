FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY HttpRequestPipeline.Api/HttpRequestPipeline.Api.csproj HttpRequestPipeline.Api/
RUN dotnet restore HttpRequestPipeline.Api/HttpRequestPipeline.Api.csproj

COPY . .
RUN dotnet publish HttpRequestPipeline.Api/HttpRequestPipeline.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Health check için curl kur
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 5161
ENTRYPOINT ["dotnet", "HttpRequestPipeline.Api.dll"]