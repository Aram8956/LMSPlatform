# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Course/Course.API/Course.API.csproj", "Services/Course/Course.API/"]
COPY ["Services/Billing/Billing.API/Billing.API.csproj", "Services/Billing/Billing.API/"]
COPY ["shared/Shared/Shared.csproj", "shared/Shared/"]
COPY ["Services/Billing/Billing.Infrastructure/Billing.Infrastructure.csproj", "Services/Billing/Billing.Infrastructure/"]
COPY ["Services/Course/Course.Aplication/Course.Aplication.csproj", "Services/Course/Course.Aplication/"]
COPY ["Services/Course/Course.Domain/Course.Domain.csproj", "Services/Course/Course.Domain/"]
COPY ["Services/Course/Course.Infrastructure/Course.Infrastructure.csproj", "Services/Course/Course.Infrastructure/"]
RUN dotnet restore "./Services/Course/Course.API/Course.API.csproj"
COPY . .
WORKDIR "/src/Services/Course/Course.API"
RUN dotnet build "./Course.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Course.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Course.API.dll"]