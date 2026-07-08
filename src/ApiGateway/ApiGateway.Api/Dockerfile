# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY . .

WORKDIR /src/ApiGateway.Api

RUN dotnet restore

RUN dotnet publish -c Release -o /app/publish


# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 7058

ENTRYPOINT ["dotnet", "ApiGateway.Api.dll"]