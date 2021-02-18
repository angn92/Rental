

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Rental.Api/Rental.Api.csproj", "Rental.Api/"]
COPY ["Rental.Infrastructure/Rental.Infrastructure.csproj", "Rental.Infrastructure/"]
COPY ["Rental.Core/Rental.Core.csproj", "Rental.Core/"]
RUN dotnet restore "Rental.Api/Rental.Api.csproj"
COPY . .
WORKDIR "/src/Rental.Api"
RUN dotnet build "Rental.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rental.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rental.Api.dll"]