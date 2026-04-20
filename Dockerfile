# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar arquivos de projeto e restaurar dependências
COPY ["SGF.API/SGF.API.csproj", "SGF.API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "SGF.API/SGF.API.csproj"

# Copiar todo o código fonte e fazer o build
COPY . .
WORKDIR "/src/SGF.API"
RUN dotnet build "SGF.API.csproj" -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish "SGF.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final - runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGF.API.dll"]
