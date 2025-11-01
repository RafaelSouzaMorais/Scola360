# Linux-based Dockerfile for Linux containers
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Scola360.Academico/Scola360.Academico.csproj", "Scola360.Academico/"]
COPY ["Scola360.Academico.Application/Scola360.Academico.Application.csproj", "Scola360.Academico.Application/"]
COPY ["Scola360.Academico.Domain/Scola360.Academico.Domain.csproj", "Scola360.Academico.Domain/"]
COPY ["Scola360.Academico.Infrastructure/Scola360.Academico.Infrastructure.csproj", "Scola360.Academico.Infrastructure/"]

RUN dotnet restore "Scola360.Academico/Scola360.Academico.csproj"

COPY . .
WORKDIR "/src/Scola360.Academico"
RUN dotnet build "Scola360.Academico.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Scola360.Academico.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Scola360.Academico.dll"]