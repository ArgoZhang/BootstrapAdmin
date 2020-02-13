﻿function installDB() {
    write-host "init sqlserver database..." -ForegroundColor Cyan
    $startPath = "$($env:appveyor_build_folder)\db\SqlServer"
    $sqlInstance = "(local)\SQL2017"
    $outFile = join-path $startPath "output.log"
    $sqlFile = join-path $startPath "Install.sql"
    $initFile = join-path $startPath "InitData.sql"

    sqlcmd -S "$sqlInstance" -U sa -P Password12! -i "$sqlFile" -i "$initFile" -o "$outFile"

    write-host "init mysql database..." -ForegroundColor Cyan
    $env:MYSQL_PWD="Password12!"
    $mysql = '"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe"'
    $cmd = $mysql + ' -e "create database BootstrapAdmin;" -uroot'
    cmd.exe /c $cmd

    $startPath = "$($env:appveyor_build_folder)\db\MySQL"
    $para = ' -hlocalhost -uroot -DBootstrapAdmin < '
    $sqlFile = join-path $startPath "Install.sql"
    $cmd = $mysql + $para + $sqlFile
    cmd.exe /c $cmd

    $initFile = join-path $startPath "InitData.sql"
    $cmd = $mysql + $para + $initFile
    cmd.exe /c $cmd   

    write-host "init mongodb data..." -ForegroundColor Cyan
    $initFolder = "$($env:appveyor_build_folder)\db\MongoDB"
    cd $initFolder

    cmd.exe /c "C:\mongodb\bin\mongo init.js"

    $cmd = 'C:\mongodb\bin\mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"'
    iex "& $cmd"

    cd $($env:appveyor_build_folder)
}

function runUnitTest() {
    write-host "dotnet test test\UnitTest" -ForegroundColor Cyan
    dotnet test "test\UnitTest" /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Include="[Bootstrap.Admin*]*%2c[Bootstrap.DataAccess*]*" /p:Exclude="[*]*Program%2c[*]*Startup%2c[Bootstrap.DataAccess*]*AutoDB*%2c[Bootstrap.DataAccess]*WeChatHelper" /p:ExcludeByFile="**/SMSExtensions.cs%2c**/Helper/OAuthHelper.cs%2c**/Extensions/CloudLoggerExtensions.cs%2c**/Extensions/AutoGenerateDatabaseExtensions.cs%2c**/Api/HealthsController.cs%2c**/Pages/**%2c**/DBLogTask.cs%2c**/AutoDbHelper.cs" /p:CoverletOutput=..\..\
}

function installCoveralls() {
    write-host "install coveralls.net tools" -ForegroundColor Cyan
    dotnet tool install coveralls.net --tool-path ".\tools"
}

function reportCoveralls() {
    Set-AppveyorBuildVariable COVERALLS_REPO_TOKEN $($env:COVERALLS_REPO_TOKEN)

    write-host "report UnitTest with Coveralls" -ForegroundColor Cyan
    .\tools\csmacnz.Coveralls.exe --opencover -i coverage.opencover.xml --useRelativePaths
}

function installCodecov {
    write-host "install Codecov.Tool tools" -ForegroundColor Cyan
    dotnet tool install Codecov.Tool --tool-path ".\tools"
}

function reportCodecov() {
    Set-AppveyorBuildVariable CODECOV_TOKEN $($env:CODECOV_TOKEN)
    Set-AppveyorBuildVariable CI $($env:CI)
    Set-AppveyorBuildVariable APPVEYOR $($env:Appveyor)

    $coverageFile = Test-Path coverage.opencover.xml
    if ($coverageFile) {
        write-host "report UnitTest with Codecov" -ForegroundColor Cyan
        .\tools\codecov -f coverage.opencover.xml
    }
}

$branch = $($env:APPVEYOR_REPO_BRANCH)
if ($branch -ne "dev") {
    installDB
    installCoveralls
    installCodecov
    runUnitTest
    reportCoveralls
    reportCodecov
}
