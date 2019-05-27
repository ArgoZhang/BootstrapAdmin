#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk-nanoserver-1803 AS build
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
COPY --from=build ["/src/Bootstrap.Admin/BootstrapAdmin.db", "./BootstrapAdmin.db"]
COPY --from=build ["/src/Scripts/Longbow.lic", "./Longbow.lic"]
ENTRYPOINT ["dotnet", "Bootstrap.Admin.dll"]