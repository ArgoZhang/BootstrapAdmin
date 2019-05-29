FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Bootstrap.Admin/Bootstrap.Admin.csproj", "Bootstrap.Admin/"]
COPY ["Bootstrap.DataAccess/Bootstrap.DataAccess.csproj", "Bootstrap.DataAccess/"]
RUN dotnet restore "Bootstrap.Admin/Bootstrap.Admin.csproj"
COPY . .
WORKDIR "/src/Bootstrap.Admin"
RUN dotnet build "Bootstrap.Admin.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Bootstrap.Admin.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=publish ["/src/Bootstrap.Admin/BootstrapAdmin.db", "./BootstrapAdmin.db"]
COPY --from=publish ["/src/Scripts/Longbow.lic", "./Longbow.lic"]
ENTRYPOINT ["dotnet", "Bootstrap.Admin.dll"]