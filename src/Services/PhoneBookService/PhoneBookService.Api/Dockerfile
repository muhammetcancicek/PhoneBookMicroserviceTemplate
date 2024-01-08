#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/PhoneBookService/PhoneBookService.Api/PhoneBookService.Api.csproj", "src/Services/PhoneBookService/PhoneBookService.Api/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Application/PhoneBookService.Application.csproj", "src/Services/PhoneBookService/PhoneBookService.Application/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Domain/PhoneBookService.Domain.csproj", "src/Services/PhoneBookService/PhoneBookService.Domain/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Infrastructure/PhoneBookService.Infrastructure.csproj", "src/Services/PhoneBookService/PhoneBookService.Infrastructure/"]
RUN dotnet restore "./src/Services/PhoneBookService/PhoneBookService.Api/./PhoneBookService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/PhoneBookService/PhoneBookService.Api"
RUN dotnet build "./PhoneBookService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PhoneBookService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhoneBookService.Api.dll"]