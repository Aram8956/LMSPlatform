FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y iputils-ping netcat-openbsd

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/Billing/Billing.API/Billing.API.csproj", "Services/Billing/Billing.API/"]
COPY ["Services/Billing/Billing.Infrastructure/Billing.Infrastructure.csproj", "Services/Billing/Billing.Infrastructure/"]
COPY ["shared/Shared/Shared.csproj", "shared/Shared/"]

RUN dotnet restore "Services/Billing/Billing.API/Billing.API.csproj"

COPY Services/Billing/Billing.API ./Services/Billing/Billing.API
COPY Services/Billing/Billing.Infrastructure ./Services/Billing/Billing.Infrastructure
COPY shared/Shared ./shared/Shared

WORKDIR "/src/Services/Billing/Billing.API"
RUN dotnet build "Billing.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Billing.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Billing.API.dll"]
