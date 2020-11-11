#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9026
EXPOSE 9026

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["endpoints/Abacuza.Endpoints.ApiService/Abacuza.Endpoints.ApiService.csproj", "endpoints/Abacuza.Endpoints.ApiService/"]
COPY ["common/Abacuza.Common/Abacuza.Common.csproj", "common/Abacuza.Common/"]
COPY ["endpoints/Abacuza.Endpoints/Abacuza.Endpoints.csproj", "endpoints/Abacuza.Endpoints/"]
COPY ["endpoints/Abacuza.Endpoints.Input/Abacuza.Endpoints.Input.csproj", "endpoints/Abacuza.Endpoints.Input/"]
RUN dotnet restore "endpoints/Abacuza.Endpoints.ApiService/Abacuza.Endpoints.ApiService.csproj"
RUN dotnet restore "endpoints/Abacuza.Endpoints.Input/Abacuza.Endpoints.Input.csproj"
COPY . .
WORKDIR "/src/endpoints/Abacuza.Endpoints.ApiService"
RUN dotnet build "Abacuza.Endpoints.ApiService.csproj" -c Release -o /app/build
WORKDIR "/src/endpoints/Abacuza.Endpoints.Input"
RUN dotnet build "Abacuza.Endpoints.Input.csproj" -c Release -o /app/plugins/endpoints

FROM build AS publish
WORKDIR "/src/endpoints/Abacuza.Endpoints.ApiService"
RUN dotnet publish "Abacuza.Endpoints.ApiService.csproj" -c Release -o /app/publish
WORKDIR "/src/endpoints/Abacuza.Endpoints.Input"
RUN dotnet publish "Abacuza.Endpoints.Input.csproj" -c Release -o /app/publish/plugins/endpoints

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Abacuza.Endpoints.ApiService.dll"]
