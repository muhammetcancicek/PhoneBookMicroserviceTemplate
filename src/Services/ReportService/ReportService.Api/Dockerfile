#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5050
EXPOSE 5051

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/ReportService/ReportService.Api/ReportService.Api.csproj", "src/Services/ReportService/ReportService.Api/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Infrastructure/PhoneBookService.Infrastructure.csproj", "src/Services/PhoneBookService/PhoneBookService.Infrastructure/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Application/PhoneBookService.Application.csproj", "src/Services/PhoneBookService/PhoneBookService.Application/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Domain/PhoneBookService.Domain.csproj", "src/Services/PhoneBookService/PhoneBookService.Domain/"]
COPY ["src/Services/ReportService/ReportService.Application/ReportService.Application.csproj", "src/Services/ReportService/ReportService.Application/"]
COPY ["src/Messaging/PhoneBookService.Messaging/PhoneBookService.Messaging.csproj", "src/Messaging/PhoneBookService.Messaging/"]
COPY ["src/Services/ReportService/ReportService.Domain/ReportService.Domain.csproj", "src/Services/ReportService/ReportService.Domain/"]
COPY ["src/Services/ReportService/ReportService.Infrastructure/ReportService.Infrastructure.csproj", "src/Services/ReportService/ReportService.Infrastructure/"]
RUN dotnet restore "./src/Services/ReportService/ReportService.Api/./ReportService.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/ReportService/ReportService.Api"
RUN dotnet build "./ReportService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReportService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportService.Api.dll"]