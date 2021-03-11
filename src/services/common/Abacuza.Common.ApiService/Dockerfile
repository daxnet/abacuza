#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9025
EXPOSE 9025

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["common/Abacuza.Common/Abacuza.Common.csproj", "common/Abacuza.Common/"]
COPY ["common/Abacuza.Common.ApiService/Abacuza.Common.ApiService.csproj", "common/Abacuza.Common.ApiService/"]
RUN dotnet restore "common/Abacuza.Common.ApiService/Abacuza.Common.ApiService.csproj"
COPY . .
WORKDIR "/src/common/Abacuza.Common.ApiService"
# RUN dotnet build "Abacuza.Common.ApiService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -p:IncludeLicenseWhenGeneratePackage=false "Abacuza.Common.ApiService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Abacuza.Common.ApiService.dll"]