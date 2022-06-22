FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/ChallengeOrder/ChallengeOrder.Host/ChallengeOrder.Host.csproj", "ChallengeOrder.Host/"]
RUN dotnet restore "Services/ChallengeOrder/ChallengeOrder.Host/ChallengeOrder.Host.csproj"
COPY . .
WORKDIR "/src/ChallengeOrder.Host"
RUN dotnet build "ChallengeOrder.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChallengeOrder.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChallengeOrder.Host.dll"]
