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

# Establecer el directorio de trabajo
WORKDIR /app

# Instalar Python y pip
RUN apt-get update && apt-get install -y python3 python3-pip

# Copiar los archivos de Python y el archivo de dependencias
COPY PythonScripts /app/PythonScripts

# Instalar dependencias de Python
RUN pip3 install --no-cache-dir -r /app/PythonScripts/requirements.txt

# Copiar los archivos publicados de la app .NET
COPY --from=build /publish .

# Definir el comando de entrada para .NET
ENTRYPOINT ["dotnet", "PharmaStock.dll"]