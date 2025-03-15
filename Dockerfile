# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar archivos del proyecto y restaurar dependencias
COPY *.sln .
COPY PharmaStock/*.csproj ./PharmaStock/
RUN dotnet restore

# Copiar el resto del código fuente y compilar
COPY . .
WORKDIR /app/PharmaStock
RUN dotnet publish -c Release -o /publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

# Configurar el puerto de ejecución
EXPOSE 8080

# Comando para ejecutar la app
ENTRYPOINT ["dotnet", "PharmaStock.dll"]
