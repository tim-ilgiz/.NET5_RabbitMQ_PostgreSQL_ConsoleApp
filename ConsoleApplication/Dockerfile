FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source
EXPOSE 80

COPY ["ConsoleApplication/ConsoleApplication.csproj", "ConsoleApplication/"]
COPY ["SmsProcessingService.sln", "SmsProcessingService.sln"]
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["SmsProcessingService.Domain/SmsProcessingService.Domain.csproj", "SmsProcessingService.Domain/"]
COPY ["SmsProcessingService.Infrastructure/SmsProcessingService.Infrastructure.csproj", "SmsProcessingService.Infrastructure/"]
COPY ["SmsProcessingService.Service/SmsProcessingService.Service.csproj", "SmsProcessingService.Service/"]
COPY ["SmsProcessingService.Persistence/SmsProcessingService.Persistence.csproj", "SmsProcessingService.Persistence/"]

RUN dotnet restore "SmsProcessingService.sln"

COPY . .
WORKDIR "/source/ConsoleApplication"
RUN dotnet publish "ConsoleApplication.csproj" -c Release -o /app --no-cache /restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet", "ConsoleApplication.dll" ]