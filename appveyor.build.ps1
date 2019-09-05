function runCmd ($cmd) {
    write-host $cmd -ForegroundColor Cyan
    cmd.exe /c $cmd
}
runCmd "dotnet build src\admin\Bootstrap.Admin"
runCmd "dotnet publish src\admin\Bootstrap.Admin --configuration Release --no-restore"

$publishFolder = "$($env:appveyor_build_folder)\src\admin\Bootstrap.Admin\bin\Release\netcoreapp2.2\publish"
$licFile = "$($env:appveyor_build_folder)\src\admin\keys\Longbow.lic"
write-host "copy file $licFile" -ForegroundColor Cyan
xcopy $licFile $publishFolder /y

$dbFile = "$($env:appveyor_build_folder)\src\admin\Bootstrap.Admin\BootstrapAdmin.db"
write-host "copy file $dbFile" -ForegroundColor Cyan
xcopy $dbFile $publishFolder /y