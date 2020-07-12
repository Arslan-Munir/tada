FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Copy csproj and restore as distinct layers
COPY *.csproj /build/
RUN dotnet restore ./build/Tada.API.csproj

# Copy everything else and build
COPY . ./build/
WORKDIR /build

RUN dotnet publish ./Tada.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /build/out .
# ENTRYPOINT ["dotnet", "Tada.API.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Tada.API.dll