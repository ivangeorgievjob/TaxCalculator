# Use the official ASP.NET Core SDK image from Microsoft
# for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["TaxCalculator/TaxCalculator.csproj", "TaxCalculator/"]
RUN dotnet restore "TaxCalculator/TaxCalculator.csproj"

# Copy the remaining source code and build the application
COPY . .
WORKDIR "/src/TaxCalculator"
RUN dotnet build "TaxCalculator.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "TaxCalculator.csproj" -c Release -o /app/publish

# Final stage/image for running the published application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
EXPOSE 80

# Copy the published output from the previous stage
COPY --from=publish /app/publish .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "TaxCalculator.dll"]
