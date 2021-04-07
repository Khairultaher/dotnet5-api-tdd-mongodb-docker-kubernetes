#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /app
#COPY ["Catalog.Api.csproj", "./"]
#RUN dotnet restore "Catalog.Api.csproj"
#COPY . .
#RUN dotnet build "Catalog.Api.csproj" -c Release -o /out
#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
#WORKDIR /app
#COPY --from=build /out ./
#ENTRYPOINT ["dotnet", "Catalog.Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Catalog.Api.csproj", "./"]
RUN dotnet restore "Catalog.Api.csproj"
COPY . .
#WORKDIR "/src/."
#RUN dotnet build "Catalog.Api.csproj" -c Release -o /app/build
#FROM build AS publish
RUN dotnet publish "Catalog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]



