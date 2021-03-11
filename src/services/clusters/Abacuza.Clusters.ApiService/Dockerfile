#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9023
EXPOSE 9023

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["clusters/Abacuza.Clusters.ApiService/Abacuza.Clusters.ApiService.csproj", "clusters/Abacuza.Clusters.ApiService/"]
COPY ["common/Abacuza.Common/Abacuza.Common.csproj", "common/Abacuza.Common/"]
COPY ["common/Abacuza.DataAccess.Mongo/Abacuza.DataAccess.Mongo.csproj", "common/Abacuza.DataAccess.Mongo/"]
COPY ["clusters/Abacuza.Clusters.Common/Abacuza.Clusters.Common.csproj", "clusters/Abacuza.Clusters.Common/"]
COPY ["common/Abacuza.DataAccess.DistributedCached/Abacuza.DataAccess.DistributedCached.csproj", "common/Abacuza.DataAccess.DistributedCached/"]
COPY ["clusters/Abacuza.Clusters.Spark/Abacuza.Clusters.Spark.csproj", "clusters/Abacuza.Clusters.Spark/"]
RUN dotnet restore "clusters/Abacuza.Clusters.ApiService/Abacuza.Clusters.ApiService.csproj"
RUN dotnet restore "clusters/Abacuza.Clusters.Spark/Abacuza.Clusters.Spark.csproj"
COPY . .
#WORKDIR "/src/clusters/Abacuza.Clusters.ApiService"
#RUN dotnet build "Abacuza.Clusters.ApiService.csproj" -c Release -o /app/build
#WORKDIR "/src/clusters/Abacuza.Clusters.Spark"
#RUN dotnet build "Abacuza.Clusters.Spark.csproj" -c Release -o /app/plugins/clusters/spark

FROM build AS publish
WORKDIR "/src/clusters/Abacuza.Clusters.ApiService"
RUN dotnet publish -p:IncludeLicenseWhenGeneratePackage=false "Abacuza.Clusters.ApiService.csproj" -c Release -o /app/publish
WORKDIR "/src/clusters/Abacuza.Clusters.Spark"
RUN dotnet publish -p:IncludeLicenseWhenGeneratePackage=false "Abacuza.Clusters.Spark.csproj" -c Release -o /app/publish/plugins/clusters/spark

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Abacuza.Clusters.ApiService.dll"]