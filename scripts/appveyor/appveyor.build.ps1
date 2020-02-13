function runCmd ($cmd) {
    write-host $cmd -ForegroundColor Cyan
    cmd.exe /c $cmd
}
runCmd "dotnet build src\admin\Bootstrap.Admin"
runCmd "dotnet publish src\admin\Bootstrap.Admin --configuration Release --no-restore"
