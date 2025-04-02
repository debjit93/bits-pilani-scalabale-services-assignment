FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["task-service.csproj", "./"]
RUN dotnet restore "./task-service.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "task-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "task-service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "task-service.dll"]
