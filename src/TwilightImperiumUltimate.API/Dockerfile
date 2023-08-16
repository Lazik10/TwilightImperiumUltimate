#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj", "TwilightImperiumUltimate.API/"]
RUN dotnet restore "TwilightImperiumUltimate.API/TwilightImperiumUltimate.API.csproj"
COPY . .
WORKDIR "/src/TwilightImperiumUltimate.API"
RUN dotnet build "TwilightImperiumUltimate.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwilightImperiumUltimate.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwilightImperiumUltimate.API.dll"]