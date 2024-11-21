# Multistage Dockerfile
# Stage 1: Base Image is mcr.microsoft.com/dotnet/sdk:8.0 since the webbapp is built using .NET SDK 8.0
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG: pass at build-time to the builder with the docker build command
# using the --build-arg <varname>=<value> flag.
ARG TARGETARCH

# Set the working directory, where we want to copy the files to
WORKDIR /source

# Copy the project file and restore the dependencies
COPY *.csproj .
RUN dotnet restore 

# Copy the rest of the files and build the application
COPY . .
RUN dotnet publish FoodProject.csproj -c Release -a $TARGETARCH -o /app
# -a $TARGETARCH specify the architecture
# -o /app output directory
# -c Release specify the configuration to build the project in

# stage 2: final image, which is the runtime image
# Image use is a more lightweight image mcr.microsoft.com/dotnet/aspnet:8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0
# Exposing port 8080
EXPOSE 8080
# Host machine port 5050 is mapped to container port 8080
WORKDIR /app
# Copy the files from the previous stage to the current stage
# name of the previous stage is build
COPY --from=build /app .
# Set it to run in development mode
# ENV ASPNETCORE_ENVIRONMENT=Development

# Set the entry point
ENTRYPOINT ["dotnet", "backendassign2.dll"]
# An entry point is the command that is executed when the container starts
# Should have the same name as the folder the Dockerfile is in.

# docker build -t foodproject .
# docker run --rm -p 5050:8080 foodproject
# docker run --rm -p 5050:8080 -e ASPNETCORE_ENVIRONMENT=Development foodproject