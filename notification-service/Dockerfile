FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["notification-service.csproj", "./"]
RUN dotnet restore "./notification-service.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "notification-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "notification-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "notification-service.dll"]
