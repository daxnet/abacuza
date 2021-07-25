#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9050
EXPOSE 9050

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["auth/Abacuza.Services.Identity/Abacuza.Services.Identity.csproj", "auth/Abacuza.Services.Identity/"]
RUN dotnet restore "auth/Abacuza.Services.Identity/Abacuza.Services.Identity.csproj"
COPY . .

FROM build AS publish
WORKDIR "/src/auth/Abacuza.Services.Identity"
RUN dotnet publish "Abacuza.Services.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "Abacuza.Services.Identity.dll"]
