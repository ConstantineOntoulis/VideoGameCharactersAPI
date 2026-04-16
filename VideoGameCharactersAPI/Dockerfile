# Build stage: use the .NET SDK image to restore, build, and publish the API.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the API project file first so Docker can cache restore layers efficiently.
COPY ["VideoGameCharactersAPI/VideoGameCharactersAPI.csproj", "VideoGameCharactersAPI/"]

# Restore NuGet packages for the API project.
RUN dotnet restore "VideoGameCharactersAPI/VideoGameCharactersAPI.csproj"

# Copy the rest of the repository into the build context.
COPY . .

# Move into the API project folder.
WORKDIR /src/VideoGameCharactersAPI

# Publish the API into a self-contained output folder for the runtime image.
RUN dotnet publish "VideoGameCharactersAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage: use the smaller ASP.NET runtime image to run the published app.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Copy the published output from the build stage.
COPY --from=build /app/publish .

# Expose the HTTP port used by the container.
EXPOSE 8080

# Tell ASP.NET Core to listen on port 8080 inside the container.
ENV ASPNETCORE_URLS=http://+:8080

# Start the API when the container launches.
ENTRYPOINT ["dotnet", "VideoGameCharactersAPI.dll"]