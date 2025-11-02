# Linux-based Dockerfile for Linux containers
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos .csproj a partir da raiz do contexto
COPY ["BackEnd/Scola360/Scola360.Academico/Scola360.Academico.csproj", "Scola360.Academico/"]
COPY ["BackEnd/Scola360/Scola360.Academico.Application/Scola360.Academico.Application.csproj", "Scola360.Academico.Application/"]
COPY ["BackEnd/Scola360/Scola360.Academico.Domain/Scola360.Academico.Domain.csproj", "Scola360.Academico.Domain/"]
COPY ["BackEnd/Scola360/Scola360.Academico.Infrastructure/Scola360.Academico.Infrastructure.csproj", "Scola360.Academico.Infrastructure/"]
RUN dotnet restore "Scola360.Academico/Scola360.Academico.csproj"

# Copia todo o código fonte
COPY ["BackEnd/Scola360/", "."]

WORKDIR "/src/Scola360.Academico"
RUN dotnet build "Scola360.Academico.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Scola360.Academico.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Build EF Core migrations bundle
FROM build AS efbundle
RUN dotnet tool update --global dotnet-ef \
 && export PATH="$PATH:/root/.dotnet/tools" \
 && dotnet ef migrations bundle \
      -p ../Scola360.Academico.Infrastructure/Scola360.Academico.Infrastructure.csproj \
      -s ./Scola360.Academico.csproj \
      -o /app/migrate \
      --force

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=efbundle /app/migrate /app/migrate
COPY --from=build /src/Scola360.Academico/docker-entrypoint.sh /app/docker-entrypoint.sh

RUN apt-get update && apt-get install -y dos2unix curl \
 && dos2unix /app/docker-entrypoint.sh \
 && chmod +x /app/docker-entrypoint.sh \
 && apt-get remove -y dos2unix \
 && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["/app/docker-entrypoint.sh"]