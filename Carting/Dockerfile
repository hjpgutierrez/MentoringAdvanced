# Use an official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
# Expose the port that the API runs on
EXPOSE 8090

# Copy the centralized props files
COPY Directory.Build.props Directory.Build.props
COPY Directory.Packages.props Directory.Packages.props

# Copy wait-for-it.sh into the container
COPY wait-for-it.sh /usr/bin/wait-for-it.sh
RUN chmod +x /usr/bin/wait-for-it.sh

# Copy the CSPROJ file and restore dependencies
COPY Carting/*.csproj ./Carting/
RUN dotnet restore Carting/Carting.csproj

# Copy the rest of the application files
COPY . .
COPY Carting/. ./Carting/
WORKDIR /app/Carting
RUN dotnet publish -c Release -o out

# Use a runtime image for the final build
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/Carting/out ./

# Run the application
ENTRYPOINT ["dotnet", "Carting.dll"]
