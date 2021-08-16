#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9024
EXPOSE 9024

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["jobs/Abacuza.Jobs.ApiService/Abacuza.Jobs.ApiService.csproj", "jobs/Abacuza.Jobs.ApiService/"]
COPY ["common/Abacuza.Common/Abacuza.Common.csproj", "common/Abacuza.Common/"]
COPY ["common/Abacuza.DataAccess.Mongo/Abacuza.DataAccess.Mongo.csproj", "common/Abacuza.DataAccess.Mongo/"]
COPY ["common/Abacuza.DataAccess.DistributedCached/Abacuza.DataAccess.DistributedCached.csproj", "common/Abacuza.DataAccess.DistributedCached/"]
RUN dotnet restore "jobs/Abacuza.Jobs.ApiService/Abacuza.Jobs.ApiService.csproj"
COPY . .

FROM build AS publish
WORKDIR "/src/jobs/Abacuza.Jobs.ApiService"
RUN dotnet publish -p:IncludeLicenseWhenGeneratePackage=false "Abacuza.Jobs.ApiService.csproj" -c Release -o /app/publish

FROM base AS final
COPY ["jobs/Abacuza.Jobs.ApiService/wait-for-it.sh", "/wait-for-it.sh"]
RUN chmod +x /wait-for-it.sh
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Abacuza.Jobs.ApiService.dll"]