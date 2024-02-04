# Use the official Microsoft .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy the solution file and the specific project file for restoration
COPY *.sln ./
COPY Nums2Words.Web/Nums2Words.Web.csproj Nums2Words.Web/
COPY Nums2Words.AppServices/Nums2Words.AppServices.csproj Nums2Words.AppServices/
COPY Nums2Words.Domain/Nums2Words.Domain.csproj Nums2Words.Domain/
COPY Nums2Words.Dtos/Nums2Words.Dtos.csproj Nums2Words.Dtos/
COPY Nums2Words.Tests/Nums2Words.Tests.csproj Nums2Words.Tests/

# Restore the project's dependencies
RUN dotnet restore Nums2Words.Web/Nums2Words.Web.csproj

# Copy the rest of the source code
COPY Nums2Words.Web/ Nums2Words.Web/
COPY Nums2Words.AppServices/ Nums2Words.AppServices/
COPY Nums2Words.Domain/ Nums2Words.Domain/
COPY Nums2Words.Dtos/ Nums2Words.Dtos/
COPY Nums2Words.Tests/ Nums2Words.Tests/

# Install Node.js
RUN apt-get update && apt-get install -y \
    software-properties-common \
    npm
RUN npm install npm@latest -g && \
    npm install n -g && \
    n latest

# Set the working directory to the project directory and publish the app
WORKDIR /app/Nums2Words.Web
RUN dotnet publish -c Release -o out

# Use the ASP.NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/Nums2Words.Web/out .
# Copy the built React app to the wwwroot directory of the ASP.NET app
COPY --from=build-env /app/Nums2Words.Web/ClientApp/build ./wwwroot

# Expose the port the app runs on
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "Nums2Words.Web.dll"]
