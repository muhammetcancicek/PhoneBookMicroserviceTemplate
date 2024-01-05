#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReportService.Api/ReportService.Api.csproj", "ReportService.Api/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Infrastructure/PhoneBookService.Infrastructure.csproj", "src/Services/PhoneBookService/PhoneBookService.Infrastructure/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Application/PhoneBookService.Application.csproj", "src/Services/PhoneBookService/PhoneBookService.Application/"]
COPY ["src/Services/PhoneBookService/PhoneBookService.Domain/PhoneBookService.Domain.csproj", "src/Services/PhoneBookService/PhoneBookService.Domain/"]
COPY ["src/Services/ReportService.Application/ReportService.Application.csproj", "src/Services/ReportService.Application/"]
COPY ["src/Messaging/PhoneBookService.Messaging/PhoneBookService.Messaging.csproj", "src/Messaging/PhoneBookService.Messaging/"]
COPY ["src/Services/ReportService/ReportService.Domain/ReportService.Domain.csproj", "src/Services/ReportService/ReportService.Domain/"]
COPY ["src/Services/ReportService/ReportService.Infrastructure/ReportService.Infrastructure.csproj", "src/Services/ReportService/ReportService.Infrastructure/"]
RUN dotnet restore "./ReportService.Api/./ReportService.Api.csproj"
COPY . .
WORKDIR "/src/ReportService.Api"
RUN dotnet build "./ReportService.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReportService.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReportService.Api.dll"]