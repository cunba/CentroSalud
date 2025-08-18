# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos todo el proyecto al contenedor
COPY . .

# Restauramos dependencias y publicamos
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app

# Copiamos los binarios publicados
COPY --from=build /app/publish .
# Copiamos el storage en la capreta de la app
COPY /Storage/* /app/Storage/

# Exponer puerto si tu app lo requiere
EXPOSE 5540

# Carpeta de datos persistentes
VOLUME ["/app/data"]

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "CentroSalud.dll"]
