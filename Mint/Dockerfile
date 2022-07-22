FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Mint/Mint.csproj", "Mint/"]
RUN dotnet restore "Mint/Mint.csproj"
COPY . .
WORKDIR "/src/Mint"
RUN dotnet build "Mint.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mint.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mint.dll"]
