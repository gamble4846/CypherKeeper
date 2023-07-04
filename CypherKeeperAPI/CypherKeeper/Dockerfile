FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 4846

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/CypherKeeper.API/CypherKeeper.API.csproj", "."]
RUN dotnet restore "./CypherKeeper.API.csproj"

COPY ["/CypherKeeper.AuthLayer/CypherKeeper.AuthLayer.csproj", "."]
RUN dotnet restore "./CypherKeeper.AuthLayer.csproj"

COPY ["/CypherKeeper.DataAccess/CypherKeeper.DataAccess.SQL.csproj", "."]
RUN dotnet restore "./CypherKeeper.DataAccess.SQL.csproj"

COPY ["/CypherKeeper.DataAccess.GoogleSheets/CypherKeeper.DataAccess.GoogleSheets.csproj", "."]
RUN dotnet restore "./CypherKeeper.DataAccess.GoogleSheets.csproj"

COPY ["/CypherKeeper.DataAccess.MongoDB/CypherKeeper.DataAccess.MongoDB.csproj", "."]
RUN dotnet restore "./CypherKeeper.DataAccess.MongoDB.csproj"

COPY ["/CypherKeeper.Manager/CypherKeeper.Manager.csproj", "."]
RUN dotnet restore "./CypherKeeper.Manager.csproj"

COPY ["/CypherKeeper.Model/CypherKeeper.Model.csproj", "."]
RUN dotnet restore "./CypherKeeper.Model.csproj"

COPY ["/CypherKeeper.Utility/CypherKeeper.Utility.csproj", "."]
RUN dotnet restore "./CypherKeeper.Utility.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "CypherKeeper.API/CypherKeeper.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CypherKeeper.API/CypherKeeper.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CypherKeeper.API.dll"]
