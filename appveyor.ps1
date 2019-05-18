if ("$($env:APPVEYOR_REPO_BRANCH)" -eq "master")
{
	.\DatabaseScripts\SqlServer\appveyor.ps1
	.\DatabaseScripts\MySQL\appveyor.ps1
	.\DatabaseScripts\MongoDB\appveyor.ps1

    echo "" "Install coveralls.net tools"
    dotnet tool install coveralls.net --version 1.0.0 --tool-path "./tools"

    dotnet test UnitTest --no-restore /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Include="[Bootstrap*]*" /p:ExcludeByFile="../Bootstrap.Admin/Program.cs%2c../Bootstrap.Admin/Startup.cs%2c../Bootstrap.Admin/HttpHeaderOperation.cs" /p:CoverletOutput=../
    cmd.exe /c ".\tools\csmacnz.Coveralls.exe --opencover -i coverage.opencover.xml --useRelativePaths"
}