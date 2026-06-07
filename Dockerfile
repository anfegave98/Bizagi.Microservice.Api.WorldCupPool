# ─── Etapa 1: Build ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos de solución y restaurar dependencias primero
# (aprovecha el cache de capas de Docker)
COPY *.sln ./
COPY src/Bizagi.Microservice.Api.WorldCupPool/Bizagi.Microservice.Api.WorldCupPool.csproj \
     src/Bizagi.Microservice.Api.WorldCupPool/
COPY src/Bizagi.Microservice.Api.WorldCupPool.Abstractions/Bizagi.Microservice.Api.WorldCupPool.Abstractions.csproj \
     src/Bizagi.Microservice.Api.WorldCupPool.Abstractions/
COPY src/Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object/Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object.csproj \
     src/Bizagi.Microservice.Api.WorldCupPool.Data.Transfer.Object/
COPY src/Bizagi.Microservice.Api.WorldCupPool.Entities/Bizagi.Microservice.Api.WorldCupPool.Entities.csproj \
     src/Bizagi.Microservice.Api.WorldCupPool.Entities/
COPY src/Bizagi.Microservice.Api.WorldCupPool.EntityFramework/Bizagi.Microservice.Api.WorldCupPool.EntityFramework.csproj \
     src/Bizagi.Microservice.Api.WorldCupPool.EntityFramework/
COPY src/Bizagi.Microservice.Api.WorldCupPool.Logic/Bizagi.Microservice.Api.WorldCupPool.Logic.csproj \
     src/Bizagi.Microservice.Api.WorldCupPool.Logic/

RUN dotnet restore

# Copiar el resto del código fuente
COPY src/ src/

# Publicar en modo Release
RUN dotnet publish src/Bizagi.Microservice.Api.WorldCupPool/Bizagi.Microservice.Api.WorldCupPool.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# ─── Etapa 2: Runtime ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar solo los artefactos publicados
COPY --from=build /app/publish .

# Render asigna el puerto via variable de entorno PORT
# ASP.NET Core lo lee automáticamente con ASPNETCORE_URLS
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Bizagi.Microservice.Api.WorldCupPool.dll"]
