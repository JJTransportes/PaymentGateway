FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 5400

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./", "PaymentGateway/"]
RUN dotnet restore "./PaymentGateway/PaymentGateway.csproj"
COPY . .
WORKDIR "/src/PaymentGateway"
RUN dotnet build "./PaymentGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaymentGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentGateway.dll"]