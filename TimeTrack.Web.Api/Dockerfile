FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TimeTrack.Web.Api/TimeTrack.Web.Api.csproj", "TimeTrack.Web.Api/"]
RUN dotnet restore "TimeTrack.Web.Api/TimeTrack.Web.Api.csproj"
COPY . .
WORKDIR "/src/TimeTrack.Web.Api"
RUN dotnet build "TimeTrack.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTrack.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTrack.Web.Api.dll"]
