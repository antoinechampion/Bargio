FROM mcr.microsoft.com/dotnet/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:2.1-stretch AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Bargio/Bargio.csproj" 
RUN dotnet build "Bargio/Bargio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bargio/Bargio.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bargio.dll"]