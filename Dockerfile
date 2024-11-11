# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj file and restore any dependencies
COPY ["TelegramBot2/TelegramBot2.csproj", "TelegramBot2/"]
RUN dotnet restore "TelegramBot2/TelegramBot2.csproj"

# Copy the entire source code into the container
COPY . .

# Publish the application (Release mode)
RUN dotnet publish "TelegramBot2/TelegramBot2.csproj" -c Release -o /app/publish

# Use the official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "TelegramBot2.dll"]
