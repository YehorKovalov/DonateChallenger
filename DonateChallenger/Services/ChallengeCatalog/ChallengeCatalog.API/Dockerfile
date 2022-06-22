FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/ChallengeCatalog/ChallengeCatalog.Host/ChallengeCatalog.Host.csproj", "ChallengeCatalog.Host/"]
RUN dotnet restore "Services/ChallengeCatalog/ChallengeCatalog.Host/ChallengeCatalog.Host.csproj"
COPY . .
WORKDIR "/src/ChallengeCatalog.Host"
RUN dotnet build "ChallengeCatalog.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChallengeCatalog.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChallengeCatalog.Host.dll"]
