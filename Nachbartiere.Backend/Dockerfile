#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nachbartiere.Backend/Nachbartiere.Backend.csproj", "Nachbartiere.Backend/"]
RUN dotnet restore "Nachbartiere.Backend/Nachbartiere.Backend.csproj"
COPY . .
WORKDIR "/src/Nachbartiere.Backend"
RUN dotnet build "Nachbartiere.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nachbartiere.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nachbartiere.Backend.dll"]