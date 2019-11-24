#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:3.0-aspnetcore-runtime-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:3.0-sdk-nanoserver-1903 AS build
WORKDIR /src
COPY . .

WORKDIR "src/admin/Bootstrap.Admin"
FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Bootstrap.Admin.dll"]