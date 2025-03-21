# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos del proyecto y restaurar dependencias
#COPY *.sln ./
COPY PharmaStock.csproj ./
RUN dotnet restore

# Copiar el resto del código fuente y compilar
COPY . ./
RUN dotnet publish -c Release -o /publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
# Instalar Python en la etapa de runtime
RUN apt-get update && apt-get install -y python3 python3-pip

COPY --from=build /publish .
ENTRYPOINT ["dotnet", "PharmaStock.dll"]
