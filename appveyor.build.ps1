function runCmd ($cmd) {
    write-host $cmd -ForegroundColor Cyan
    cmd.exe /c $cmd
}
runCmd "dotnet build"
runCmd "dotnet publish Bootstrap.Admin --configuration Release --no-restore"

$publishFolder = "$($env:appveyor_build_folder)\Bootstrap.Admin\bin\Release\netcoreapp2.2\publish"
$licFile = "$($env:appveyor_build_folder)\Scripts\Longbow.lic"
write-host "copy file $licFile" -ForegroundColor Cyan
xcopy $licFile $publishFolder /y

$dbFile = "$($env:appveyor_build_folder)\Bootstrap.Admin\BootstrapAdmin.db"
write-host "copy file $dbFile" -ForegroundColor Cyan
xcopy $dbFile $publishFolder /y