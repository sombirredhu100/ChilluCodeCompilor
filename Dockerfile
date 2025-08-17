# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["ChilluCodeCompilor/ChilluCodeCompilor.csproj", "./"]
RUN dotnet restore

# Copy the rest of the app and build it
COPY . .
RUN dotnet publish -c Release -o /app/publish

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Expose default port
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "ChilluCodeCompilor.dll"]
