#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443


COPY Rental.Api/*.csproj ./Rental.Api/
COPY Rental.Infrastructure/*.csproj ./Rental.Infrastructure/
COPY Rental.Core/*.csproj ./Rental.Core/
#""
#RUN dotnet restore "Rental.Api/Rental.Api.csproj"

COPY Rental.Api/ ./Rental.Api/
COPY Rental.Infrastructure/ ./Rental.Infrastructure/
COPY Rental.Core/ ./Rental.Core/

WORKDIR /app

RUN dotnet publish /app/Rental.Api/Rental.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Rental.Api.dll"]