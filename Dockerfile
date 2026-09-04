# Use .NET 10 SDK for build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and projects for restore
COPY ["AltavixAPI/AltavixAPI.sln", "AltavixAPI/"]
COPY ["AltavixAPI/AltavixAPI.csproj", "AltavixAPI/"]
COPY ["Altavix.Application/Altavix.Application.csproj", "Altavix.Application/"]
COPY ["Altavix.Domain/Altavix.Domain.csproj", "Altavix.Domain/"]
COPY ["Altavix.Persistence/Altavix.Persistence.csproj", "Altavix.Persistence/"]

RUN dotnet restore "AltavixAPI/AltavixAPI.sln"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/AltavixAPI"

# Build and publish the project
ARG APP_VERSION=0.1.0
RUN dotnet publish "AltavixAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:Version=$APP_VERSION

# Use .NET 10 Runtime for production stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AltavixAPI.dll"]