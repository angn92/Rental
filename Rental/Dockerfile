# Define base image for our image
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app

# Copy csproj and restore 
COPY *.csproj ./
RUN dotnet restore "../Rental.Api.csproj"

COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image 
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Rental.Api.dll"]