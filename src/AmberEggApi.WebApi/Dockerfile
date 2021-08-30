#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AmberEggApi.WebApi/AmberEggApi.WebApi.csproj", "AmberEggApi.WebApi/"]
COPY ["AmberEggApi.Infrastructure/AmberEggApi.Infrastructure.csproj", "AmberEggApi.Infrastructure/"]
COPY ["Api.Common.Repository.EFCore/Api.Common.Repository.EFCore.csproj", "Api.Common.Repository.EFCore/"]
COPY ["Api.Common.Repository.Contracts.Core/Api.Common.Repository.csproj", "Api.Common.Repository.Contracts.Core/"]
COPY ["Api.Common.Cqrs.Core/Api.Common.Cqrs.Core.csproj", "Api.Common.Cqrs.Core/"]
COPY ["Api.Common.Contracts/Api.Common.Contracts.csproj", "Api.Common.Contracts/"]
COPY ["AmberEggApi.Database/AmberEggApi.Database.csproj", "AmberEggApi.Database/"]
COPY ["AmberEggApi.Domain/AmberEggApi.Domain.csproj", "AmberEggApi.Domain/"]
COPY ["Api.Common.WebServer/Api.Common.WebServer.csproj", "Api.Common.WebServer/"]
COPY ["AmberEggApi.ApplicationService/AmberEggApi.ApplicationService.csproj", "AmberEggApi.ApplicationService/"]
RUN dotnet restore "AmberEggApi.WebApi/AmberEggApi.WebApi.csproj"
COPY . .
WORKDIR "/src/AmberEggApi.WebApi"
RUN dotnet build "AmberEggApi.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AmberEggApi.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AmberEggApi.WebApi.dll"]