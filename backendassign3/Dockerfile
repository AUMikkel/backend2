# Use the official ASP.NET runtime image as the base for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Set the working directory in the build stage
WORKDIR /src

# Copy the .csproj file and restore any dependencies
COPY backendassign3.csproj ./ 
RUN dotnet restore "backendassign3.csproj"

# Copy the rest of the application files
COPY . ./

# Build the application
RUN dotnet build "backendassign3.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application to a folder for deployment
FROM build AS publish
RUN dotnet publish "backendassign3.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image to be used in the running container
FROM base AS final
WORKDIR /app

# Copy the published files to the runtime container
COPY --from=publish /app/publish .

# Set the entry point for the container to run the application
ENTRYPOINT ["dotnet", "backendassign3.dll"]
