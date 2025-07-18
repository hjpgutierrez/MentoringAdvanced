# Use an official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
# Expose the port that the API runs on
EXPOSE 9080
# Copy the centralized props files
COPY Directory.Build.props Directory.Build.props
COPY Directory.Packages.props Directory.Packages.props
# Copy wait-for-it.sh into the container
COPY wait-for-it.sh /usr/bin/wait-for-it.sh
RUN chmod +x /usr/bin/wait-for-it.sh
# Copy the CSPROJ file and restore dependencies
COPY Catalog.API/*.csproj ./Catalog.API/
COPY Catalog.Domain/*.csproj ./Catalog.Domain/
COPY Catalog.Application/*.csproj ./Catalog.Application/
COPY Catalog.Infrastructure/*.csproj ./Catalog.Infrastructure/

# Restore dependencies for the solution
WORKDIR /app/Catalog.API
RUN dotnet restore

# Copy the source code for all projects
WORKDIR /app
COPY . ./

# Build and publish the API
WORKDIR /app/Catalog.API
RUN dotnet publish -c Release -o out

# Use a runtime image for the final build
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/Catalog.API/out ./



# Run the application
ENTRYPOINT ["dotnet", "Catalog.API.dll"]