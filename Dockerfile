FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj файлы всех слоев
COPY ["Presentation/TaskMnagementBackend.Api/TaskMnagementBackend.Api.csproj", "Presentation/TaskMnagementBackend.Api/"]
COPY ["Core/TaskMnagementBackend.Aplication/TaskMnagementBackend.Aplication.csproj", "Core/TaskMnagementBackend.Aplication/"]
COPY ["Core/TaskMnagementBackend.Domain/TaskMnagementBackend.Domain.csproj", "Core/TaskMnagementBackend.Domain/"]
COPY ["Infrastructure/TaskMnagementBackend.Infrastructure/TaskMnagementBackend.Infrastructure.csproj", "Infrastructure/TaskMnagementBackend.Infrastructure/"]
COPY ["Infrastructure/TaskMnagementBackend.Persistence/TaskMnagementBackend.Persistence.csproj", "Infrastructure/TaskMnagementBackend.Persistence/"]

RUN dotnet restore "Presentation/TaskMnagementBackend.Api/TaskMnagementBackend.Api.csproj"

COPY . .
WORKDIR "/src/Presentation/TaskMnagementBackend.Api"
RUN dotnet build "TaskMnagementBackend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskMnagementBackend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskMnagementBackend.Api.dll"]