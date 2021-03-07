#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9027
EXPOSE 9027

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["projects/Abacuza.Projects.ApiService/Abacuza.Projects.ApiService.csproj", "projects/Abacuza.Projects.ApiService/"]
COPY ["common/Abacuza.Common/Abacuza.Common.csproj", "common/Abacuza.Common/"]
COPY ["common/Abacuza.DataAccess.Mongo/Abacuza.DataAccess.Mongo.csproj", "common/Abacuza.DataAccess.Mongo/"]
COPY ["common/Abacuza.DataAccess.DistributedCached/Abacuza.DataAccess.DistributedCached.csproj", "common/Abacuza.DataAccess.DistributedCached/"]
RUN dotnet restore "projects/Abacuza.Projects.ApiService/Abacuza.Projects.ApiService.csproj"
COPY . .
#WORKDIR "/src/projects/Abacuza.Projects.ApiService"
#RUN dotnet build "Abacuza.Projects.ApiService.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/projects/Abacuza.Projects.ApiService"
RUN dotnet publish -p:IncludeLicenseWhenGeneratePackage=false "Abacuza.Projects.ApiService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Abacuza.Projects.ApiService.dll"]
