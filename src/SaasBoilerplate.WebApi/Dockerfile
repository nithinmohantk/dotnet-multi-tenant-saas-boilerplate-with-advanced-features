# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SaasBoilerplate.WebApi/SaasBoilerplate.WebApi.csproj", "src/SaasBoilerplate.WebApi/"]
COPY ["src/SaasBoilerplate.Infrastructure/SaasBoilerplate.Infrastructure.csproj", "src/SaasBoilerplate.Infrastructure/"]
COPY ["src/SaasBoilerplate.Application/SaasBoilerplate.Application.csproj", "src/SaasBoilerplate.Application/"]
COPY ["src/SaasBoilerplate.Domain/SaasBoilerplate.Domain.csproj", "src/SaasBoilerplate.Domain/"]
RUN dotnet restore "./src/SaasBoilerplate.WebApi/SaasBoilerplate.WebApi.csproj"
COPY . .
WORKDIR "/src/src/SaasBoilerplate.WebApi"
RUN dotnet build "./SaasBoilerplate.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SaasBoilerplate.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SaasBoilerplate.WebApi.dll"]
