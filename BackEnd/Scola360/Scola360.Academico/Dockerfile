# Linux-based Dockerfile for Linux containers
# See https://aka.ms/customizecontainer

# Base image (Linux)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SistemaAcademico/Scola360.Academico.csproj", "SistemaAcademico/"]
COPY ["Scola360.Academico.Application/Scola360.Academico.Application.csproj", "Scola360.Academico.Application/"]
COPY ["Scola360.Academico.Domain/Scola360.Academico.Domain.csproj", "Scola360.Academico.Domain/"]
COPY ["Scola360.Academico.Infrastructure/Scola360.Academico.Infrastructure.csproj", "Scola360.Academico.Infrastructure/"]
RUN dotnet restore "SistemaAcademico/Scola360.Academico.csproj"
COPY . .
WORKDIR "/src/SistemaAcademico"
RUN dotnet build "./Scola360.Academico.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Scola360.Academico.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Scola360.Academico.dll"]