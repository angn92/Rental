# Define base image for our image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY *.csproj ./

COPY . ./
RUN dotnet build -o /app
RUN dotnet publish -c Release -o /publish

# Build runtime image 
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
WORKDIR /app
COPY --from=build /publish /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "Rental.Api.dll"]