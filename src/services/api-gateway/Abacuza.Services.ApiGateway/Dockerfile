#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9099
ENV APPLICATION_NAME=abacuza
EXPOSE 9099

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["api-gateway/Abacuza.Services.ApiGateway/Abacuza.Services.ApiGateway.csproj", "api-gateway/Abacuza.Services.ApiGateway/"]
RUN dotnet restore "api-gateway/Abacuza.Services.ApiGateway/Abacuza.Services.ApiGateway.csproj"
COPY . .

FROM build AS publish
WORKDIR "/src/api-gateway/Abacuza.Services.ApiGateway"
RUN dotnet publish "Abacuza.Services.ApiGateway.csproj" -c Release -o /app/publish
RUN cp ocelot.configuration.tmpl /app/publish

FROM base AS final
RUN apt update
RUN apt install -y wget
ENV DOCKERIZE_VERSION v0.6.1
RUN wget https://github.com/jwilder/dockerize/releases/download/$DOCKERIZE_VERSION/dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && tar -C /usr/local/bin -xzvf dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz \
    && rm dockerize-linux-amd64-$DOCKERIZE_VERSION.tar.gz

WORKDIR /app
COPY --from=publish /app/publish .
CMD dockerize -template /app/ocelot.configuration.tmpl:/app/ocelot.configuration.json dotnet Abacuza.Services.ApiGateway.dll
