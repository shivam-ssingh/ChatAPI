# Containerizing .NET API: https://medium.com/@marcus121neo/asp-net-core-multi-environment-setup-with-docker-7591a872bacc
#https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-9.0
# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy .csproj and restore dependencies
COPY ["ChatAPI.csproj", "./"]
RUN dotnet restore "ChatAPI.csproj"

# Copy the entire project
COPY . .

# Build and publish
RUN dotnet publish -c Release -o /out

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Expose ports
EXPOSE 5000
EXPOSE 5001

# .NET Variables in docker: https://stackoverflow.com/a/69738798
# MongoDB Settings
ENV MongoDBSettings__ConnectionString=""
ENV MongoDBSettings__DatabaseName=""

# JWT Settings
ENV Jwt__Secret=""
ENV Jwt__Issuer=""
ENV Jwt__Audience=""
ENV Jwt__ExpirationInMinutes="60"


# Github settings
ENV GitHubOAuth__ClientId=""
ENV GitHubOAuth__ClientSecret=""
ENV GitHubOAuth__RedirectUri=""

# Run the application
ENTRYPOINT ["dotnet", "ChatAPI.dll"]
