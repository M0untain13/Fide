FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["packages", "/root/.nuget/NuGet/devexpress-local"]
RUN  dotnet nuget add source "devexpress-local" -n devexpress-local
COPY ["Fide.Blazor.Server/Fide.Blazor.Server.csproj", "Fide.Blazor.Server/"]
COPY ["Fide.Module/Fide.Module.csproj", "Fide.Module/"]
RUN dotnet restore "./Fide.Blazor.Server/Fide.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/Fide.Blazor.Server"
RUN dotnet build "Fide.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Fide.Blazor.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Fide.Blazor.Server.dll"]
