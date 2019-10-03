function installDB() {
    write-host "init sqlserver database..." -ForegroundColor Cyan
    $startPath = "$($env:appveyor_build_folder)\db\SqlServer"
    $sqlInstance = "(local)\SQL2017"
    $outFile = join-path $startPath "output.log"
    $sqlFile = join-path $startPath "Install.sql"
    $initFile = join-path $startPath "InitData.sql"

    sqlcmd -S "$sqlInstance" -U sa -P Password12! -i "$sqlFile" -i "$initFile" -o "$outFile"

    #write-host "init mysql database..." -ForegroundColor Cyan
    #$env:MYSQL_PWD="Password12!"
    #$mysql = '"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe"'
    #$cmd = $mysql + ' -e "create database BootstrapAdmin;" -uroot'
    #cmd.exe /c $cmd

    #$startPath = "$($env:appveyor_build_folder)\db\MySQL"
    #$para = ' -hlocalhost -uroot -DBootstrapAdmin < '
    #$sqlFile = join-path $startPath "Install.sql"
    #$cmd = $mysql + $para + $sqlFile
    #cmd.exe /c $cmd

    #$initFile = join-path $startPath "InitData.sql"
    #$cmd = $mysql + $para + $initFile
    #cmd.exe /c $cmd   

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
    dotnet test test\UnitTest --filter "FullyQualifiedName!~MySql" /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Include="[Bootstrap*]*" /p:ExcludeByFile="..\..\src\admin\Bootstrap.Admin\Program.cs%2c..\..\src\admin\Bootstrap.Admin\Startup.cs%2c..\..\src\admin\Bootstrap.Admin\HttpHeaderOperation.cs" /p:CoverletOutput=..\..\
}

function coverallUnitTest() {
    write-host "install coveralls.net tools" -ForegroundColor Cyan
    dotnet tool install coveralls.net --version 1.0.0 --tool-path ".\tools"
    runUnitTest
    write-host "report UnitTest with Coveralls" -ForegroundColor Cyan
    cmd.exe /c ".\tools\csmacnz.Coveralls.exe --opencover -i coverage.opencover.xml --useRelativePaths"
}

function codecovUnitTest() {
    Set-AppveyorBuildVariable COVERALLS_REPO_TOKEN $($env:COVERALLS_REPO_TOKEN)
    Set-AppveyorBuildVariable CODECOV_TOKEN $($env:CODECOV_TOKEN)

    $codecovCmd = "C:\ProgramData\chocolatey\lib\codecov\tools\codecov.exe"
    $codecov = Test-Path $codecovCmd
    if (!$codecov) {
        write-host "install codecov tools" -ForegroundColor Cyan
        choco install codecov
    }
    $coverageFile = Test-Path coverage.opencover.xml
    if (!$coverageFile) {
        runUnitTest
    }
    write-host "report UnitTest with Codecov" -ForegroundColor Cyan
    cmd.exe /c "$codecovCmd -f ""coverage.opencover.xml"""
}

$branch = $($env:APPVEYOR_REPO_BRANCH)
if ($branch -ne "dev") {
    installDB
    coverallUnitTest
    codecovUnitTest
}
