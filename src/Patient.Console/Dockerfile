FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Patient.Console.csproj", "."]
RUN dotnet restore "./Patient.Console.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Patient.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Patient.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Patient.Console.dll"]