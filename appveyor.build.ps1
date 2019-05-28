dotnet build
dotnet publish Bootstrap.Admin --configuration Release --no-restore
xcopy "$($env:appveyor_build_folder)\Scripts\Longbow.lic" "$($env:appveyor_build_folder)\Bootstrap.Admin\bin\Release\netcoreapp2.2\publish" /y
xcopy "$($env:appveyor_build_folder)\Bootstrap.Admin\BootstrapAdmin.db" "$($env:appveyor_build_folder)\Bootstrap.Admin\bin\Release\netcoreapp2.2\publish" /y