#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MP.SimpleTokens.Identity/MP.SimpleTokens.Identity.csproj", "MP.SimpleTokens.Identity/"]
RUN dotnet restore "MP.SimpleTokens.Identity/MP.SimpleTokens.Identity.csproj"
COPY . .
WORKDIR "/src/MP.SimpleTokens.Identity"
RUN dotnet build "MP.SimpleTokens.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MP.SimpleTokens.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MP.SimpleTokens.Identity.dll"]