#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CampaignKit.WorldMap.UI/CampaignKit.WorldMap.UI.csproj", "src/CampaignKit.WorldMap.UI/"]
COPY ["src/CampaignKit.WorldMap.Core/CampaignKit.WorldMap.Core.csproj", "src/CampaignKit.WorldMap.Core/"]
RUN dotnet restore "src/CampaignKit.WorldMap.UI/CampaignKit.WorldMap.UI.csproj"
COPY . .
WORKDIR "/src/src/CampaignKit.WorldMap.UI"
RUN dotnet build "CampaignKit.WorldMap.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CampaignKit.WorldMap.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CampaignKit.WorldMap.UI.dll"]