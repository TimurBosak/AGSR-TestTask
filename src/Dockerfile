FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Patient.API/Patient.API.csproj", "Patient.API/"]
COPY ["Patient.Domain/Patient.Domain.csproj", "Patient.Domain/"]
COPY ["Patient.Repositories/Patient.Repositories.csproj", "Patient.Repositories/"]
COPY ["Patient.Services/Patient.Services.csproj", "Patient.Services/"]
RUN dotnet restore "Patient.API/Patient.API.csproj"
COPY . .
WORKDIR "/src/Patient.API"
RUN dotnet build "Patient.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Patient.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Patient.API.dll"]