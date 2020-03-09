#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["eslog2_0/eslog2_0.csproj", "eslog2_0/"]
RUN dotnet restore "eslog2_0/eslog2_0.csproj"
COPY . .
WORKDIR "/src/eslog2_0"
RUN dotnet build "eslog2_0.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eslog2_0.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eslog2_0.dll"]