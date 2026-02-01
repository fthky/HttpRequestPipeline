FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY HttpRequestPipeline.Client/HttpRequestPipeline.Client.csproj HttpRequestPipeline.Client/
RUN dotnet restore HttpRequestPipeline.Client/HttpRequestPipeline.Client.csproj

COPY . .
RUN dotnet publish HttpRequestPipeline.Client/HttpRequestPipeline.Client.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HttpRequestPipeline.Client.dll"]