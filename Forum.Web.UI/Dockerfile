#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Forum.Web.UI/Forum.Web.UI.csproj", "Forum.Web.UI/"]
COPY ["Forum.Infrastructure/Forum.Infrastructure.csproj", "Forum.Infrastructure/"]
COPY ["Forum.Application/Forum.Application.csproj", "Forum.Application/"]
COPY ["Forum.Domain/Forum.Domain.csproj", "Forum.Domain/"]
RUN dotnet restore "./Forum.Web.UI/Forum.Web.UI.csproj"
COPY . .
WORKDIR "/src/Forum.Web.UI"
RUN dotnet build "./Forum.Web.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Forum.Web.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Forum.Web.UI.dll"]