FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && apt-get install -y iputils-ping netcat-openbsd

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Services/Course/Course.API/Course.API.csproj", "Services/Course/Course.API/"]
COPY ["Services/Course/Course.Aplication/Course.Aplication.csproj", "Services/Course/Course.Aplication/"]
COPY ["Services/Course/Course.Infrastructure/Course.Infrastructure.csproj", "Services/Course/Course.Infrastructure/"]
COPY ["Services/Course/Course.Domain/Course.Domain.csproj", "Services/Course/Course.Domain/"]
COPY ["shared/Shared/Shared.csproj", "shared/Shared/"]

RUN dotnet restore "Services/Course/Course.API/Course.API.csproj"

COPY Services/Course/Course.API ./Services/Course/Course.API
COPY Services/Course/Course.Aplication ./Services/Course/Course.Aplication
COPY Services/Course/Course.Infrastructure ./Services/Course/Course.Infrastructure
COPY Services/Course/Course.Domain ./Services/Course/Course.Domain
COPY shared/Shared ./shared/Shared

WORKDIR "/src/Services/Course/Course.API"
RUN dotnet build "Course.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Course.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Course.API.dll"]
