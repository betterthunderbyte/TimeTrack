FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TimeTrack.Web.Service/TimeTrack.Web.Service.csproj", "TimeTrack.Web.Service/"]
COPY ["TimeTrack.UseCase/TimeTrack.UseCase.csproj", "TimeTrack.UseCase/"]
COPY ["TimeTrack.Core/TimeTrack.Core.csproj", "TimeTrack.Core/"]
RUN dotnet restore "TimeTrack.Web.Service/TimeTrack.Web.Service.csproj"
COPY . .
WORKDIR "/src/TimeTrack.Web.Service"
RUN dotnet build "TimeTrack.Web.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTrack.Web.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTrack.Web.Service.dll"]