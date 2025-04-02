FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["analytics-service.csproj", "./"]
RUN dotnet restore "./analytics-service.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "analytics-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "analytics-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "analytics-service.dll"]
