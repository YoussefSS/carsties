# base image. Needs to match the major .net version used in the project
# as build caches this sdk so subsequent builds will be faster
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build

# a directory inside docker, not our filesystem
WORKDIR /app

# the port we'll be using internally inside docker
EXPOSE 80

# This will significantly speed up the build process:
# copy all .csproj files and restore as distinct layers. 
# Use of the same COPY command for every dockerfile in the project to take advantage of docker cashing.
# the first param is taken from our development machines filesystem, the second part is where we'll copy it to in dockers working directory (/src)
COPY Carsties.sln Carsties.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj
COPY src/BiddingService/BiddingService.csproj src/BiddingService/BiddingService.csproj
COPY src/NotificationService/NotificationService.csproj src/NotificationService/NotificationService.csproj
COPY src/Contracts/Contracts.csproj src/Contracts/Contracts.csproj
COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj
COPY tests/AuctionService.UnitTests/AuctionService.UnitTests.csproj tests/AuctionService.UnitTests/AuctionService.UnitTests.csproj
COPY tests/AuctionService.IntegrationTests/AuctionService.IntegrationTests.csproj tests/AuctionService.IntegrationTests/AuctionService.IntegrationTests.csproj


# We have access to the dotnet cli since we use the microsoft base image
# Restore package dependencies
RUN dotnet restore Carsties.sln

# Copy the app folders over
COPY src/GatewayService src/GatewayService

# Publishing what we have in the app/ directory
WORKDIR /app/src/GatewayService
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image - this package gives access to the much smaller .net runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "GatewayService.dll" ]