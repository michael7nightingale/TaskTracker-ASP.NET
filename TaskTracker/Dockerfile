FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
COPY . .
RUN dotnet restore "./TaskTracker.csproj"
RUN dotnet build "./TaskTracker.csproj" -c Release -o /build

FROM build AS publish
RUN dotnet publish "TaskTracker.csproj" -c Release -o /build /p:UseAppHost=false

FROM base AS final
WORKDIR /build
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "TaskTracker.dll"]
