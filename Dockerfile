FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Создаем структуру директорий
RUN mkdir -p ProductionChat ProductionChat.Shared

# Копируем файлы проектов
COPY ProductionChat/ProductionChat.Server.csproj ./ProductionChat/
COPY ProductionChat.Shared/ProductionChat.Shared.csproj ./ProductionChat.Shared/

# Восстанавливаем зависимости для основного проекта
RUN dotnet restore ProductionChat/ProductionChat.Server.csproj

# Копируем все исходные коды
COPY ProductionChat/ ./ProductionChat/
COPY ProductionChat.Shared/ ./ProductionChat.Shared/

# Собираем и публикуем
RUN dotnet publish ProductionChat/ProductionChat.Server.csproj -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "ProductionChat.Server.dll"]