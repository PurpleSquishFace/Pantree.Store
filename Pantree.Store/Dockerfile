#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Pantree.Store/Pantree.Store.csproj", "Pantree.Store/"]
RUN dotnet restore "Pantree.Store/Pantree.Store.csproj"
COPY . .
WORKDIR "/src/Pantree.Store"
RUN dotnet build "Pantree.Store.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pantree.Store.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pantree.Store.dll"]