FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["SistemaHostal.API/SistemaHostal.API.csproj", "SistemaHostal.API/"]
COPY ["SistemaHostal.Application/SistemaHostal.Application.csproj", "SistemaHostal.Application/"]
COPY ["SistemaHostal.Infrastructure/SistemaHostal.Infrastructure.csproj", "SistemaHostal.Infrastructure/"]
COPY ["SistemaHostal.Domain/SistemaHostal.Domain.csproj", "SistemaHostal.Domain/"]

RUN dotnet restore "SistemaHostal.API/SistemaHostal.API.csproj"

COPY . .
WORKDIR /src/SistemaHostal.API
RUN dotnet build "SistemaHostal.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SistemaHostal.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SistemaHostal.API.dll"]
