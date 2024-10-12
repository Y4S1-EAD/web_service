# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy everything to the container
COPY . .

# Restore dependencies
RUN dotnet restore "web-service.csproj" --disable-parallel

# Publish the application
RUN dotnet publish "web-service.csproj" -c Release -o /app --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build container to the runtime container
COPY --from=build /app ./

# Expose the port (5032 is your app's HTTP port)
EXPOSE 5032

# Set the entry point for the container to run your app
ENTRYPOINT ["dotnet", "web-service.dll"]
