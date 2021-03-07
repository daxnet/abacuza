#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["api-gateway/Abacuza.Services.ApiGateway/Abacuza.Services.ApiGateway.csproj", "api-gateway/Abacuza.Services.ApiGateway/"]
RUN dotnet restore "api-gateway/Abacuza.Services.ApiGateway/Abacuza.Services.ApiGateway.csproj"
COPY . .
WORKDIR "/src/api-gateway/Abacuza.Services.ApiGateway"
RUN dotnet build "Abacuza.Services.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abacuza.Services.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Abacuza.Services.ApiGateway.dll"]