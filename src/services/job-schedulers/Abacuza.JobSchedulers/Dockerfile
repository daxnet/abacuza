#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9024
EXPOSE 9024

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["job-schedulers/Abacuza.JobSchedulers/Abacuza.JobSchedulers.csproj", "job-schedulers/Abacuza.JobSchedulers/"]
COPY ["common/Abacuza.Common/Abacuza.Common.csproj", "common/Abacuza.Common/"]
COPY ["common/Abacuza.DataAccess.Mongo/Abacuza.DataAccess.Mongo.csproj", "common/Abacuza.DataAccess.Mongo/"]
COPY ["common/Abacuza.DataAccess.DistributedCached/Abacuza.DataAccess.DistributedCached.csproj", "common/Abacuza.DataAccess.DistributedCached/"]
RUN dotnet restore "job-schedulers/Abacuza.JobSchedulers/Abacuza.JobSchedulers.csproj"
COPY . .
WORKDIR "/src/job-schedulers/Abacuza.JobSchedulers"
RUN dotnet build "Abacuza.JobSchedulers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abacuza.JobSchedulers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Abacuza.JobSchedulers.dll"]