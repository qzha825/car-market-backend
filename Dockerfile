# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY *.sln .
COPY CarMarketBackend/*.csproj ./CarMarketBackend/
RUN dotnet restore CarMarketBackend/CarMarketBackend.csproj

COPY . .
RUN dotnet publish CarMarketBackend/CarMarketBackend.csproj -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "CarMarketBackend.dll"]
