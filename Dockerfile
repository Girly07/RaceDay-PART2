
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src


COPY ["RaceDay.API/RaceDay.API.csproj", "RaceDay.API/"]
COPY ["RaceDay.MVC/RaceDay.MVC.csproj", "RaceDay.MVC/"]
RUN dotnet restore "RaceDay.MVC/RaceDay.MVC.csproj"
RUN dotnet restore "RaceDay.API/RaceDay.API.csproj"


COPY . .
WORKDIR "/src/RaceDay.MVC"
RUN dotnet build "RaceDay.MVC.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "RaceDay.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDay.MVC.dll"]