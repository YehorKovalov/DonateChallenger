FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . /src
WORKDIR /src/Services/ChallengeCatalog/ChallengeCatalog.API
RUN dotnet publish -c Release -o /app 


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ChallengeCatalog.API.dll"]