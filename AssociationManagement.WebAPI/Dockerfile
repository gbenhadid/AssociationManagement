
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AssociationManagement.WebAPI/AssociationManagement.WebAPI.csproj", "AssociationManagement.WebAPI/"]
COPY ["AssociationManagement.Application/AssociationManagement.Application.csproj", "AssociationManagement.Application/"]
COPY ["AssociationManagement.Tools/AssociationManagement.Tools.csproj", "AssociationManagement.Tools/"]
COPY ["AssociationManagement.Core/AssociationManagement.Core.csproj", "AssociationManagement.Core/"]
COPY ["AssociationManagement.DataAccess/AssociationManagement.DataAccess.csproj", "AssociationManagement.DataAccess/"]
RUN dotnet restore "./AssociationManagement.WebAPI/./AssociationManagement.WebAPI.csproj"
COPY . .
WORKDIR "/src/AssociationManagement.WebAPI"
RUN dotnet build "./AssociationManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AssociationManagement.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssociationManagement.WebAPI.dll"]