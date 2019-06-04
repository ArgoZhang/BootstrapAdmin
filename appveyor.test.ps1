function installDB() {
    write-host "init sqlserver database..." -ForegroundColor Cyan
    $startPath = "$($env:appveyor_build_folder)\DatabaseScripts\SqlServer"
    $sqlInstance = "(local)\SQL2014"
    $outFile = join-path $startPath "output.log"
    $sqlFile = join-path $startPath "Install.sql"
    $initFile = join-path $startPath "InitData.sql"

    sqlcmd -S "$sqlInstance" -U sa -P Password12! -i "$sqlFile" -i "$initFile" -o "$outFile"

    write-host "init mysql database..." -ForegroundColor Cyan
    $env:MYSQL_PWD="Password12!"
    $mysql = '"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe"'
    $cmd = $mysql + ' -e "create database BootstrapAdmin;" -uroot'
    cmd.exe /c $cmd

    $startPath = "$($env:appveyor_build_folder)\DatabaseScripts\MySQL"
    $para = ' -hlocalhost -uroot -DBootstrapAdmin < '
    $sqlFile = join-path $startPath "Install.sql"
    $cmd = $mysql + $para + $sqlFile
    cmd.exe /c $cmd

    $initFile = join-path $startPath "InitData.sql"
    $cmd = $mysql + $para + $initFile
    cmd.exe /c $cmd   

    write-host "init mongodb data..." -ForegroundColor Cyan
    $initFolder = "$($env:appveyor_build_folder)\DatabaseScripts\MongoDB"
    cd $initFolder

    cmd.exe /c "C:\mongodb\bin\mongo init.js"

    $cmd = 'C:\mongodb\bin\mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"'
    iex "& $cmd"

    cd $($env:appveyor_build_folder)
}

function coverallUnitTest() {
    write-host "install coveralls.net tools" -ForegroundColor Cyan
    dotnet tool install coveralls.net --version 1.0.0 --tool-path "./tools"    

    write-host "dotnet test UnitTest with Coveralls" -ForegroundColor Cyan
    dotnet test UnitTest --no-restore /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Include="[Bootstrap*]*" /p:ExcludeByFile="../Bootstrap.Admin/Program.cs%2c../Bootstrap.Admin/Startup.cs%2c../Bootstrap.Admin/HttpHeaderOperation.cs" /p:CoverletOutput=../
    cmd.exe /c ".\tools\csmacnz.Coveralls.exe --opencover -i coverage.opencover.xml --useRelativePaths"
}

if ("$($env:APPVEYOR_REPO_BRANCH)" -eq "master") {
    installDB

    if ("$($env:APPVEYOR_REPO_PROVIDER)" -eq "gitHub") {
        coverallUnitTest
    }
    else {
        write-warning "Coveralls has been skipped because current provider is ""$($env:APPVEYOR_REPO_PROVIDER)"""
        write-host "dotnet test UnitTest without Coveralls" -ForegroundColor Cyan
        dotnet test UnitTest --no-restore
    }
}
elseif ("$($env:APPVEYOR_REPO_PROVIDER)" -eq "gitLab") {
    installDB
    coverallUnitTest

    Set-AppveyorBuildVariable COVERALLS_REPO_TOKEN $($env:COVERALLS_REPO_TOKEN_GITLAB)
    Set-AppveyorBuildVariable CODECOV_TOKEN $($env:CODECOV_TOKEN_GITLAB)

    $codecovCmd = "C:\ProgramData\chocolatey\lib\codecov\tools\codecov.exe"
    $codecov = Test-Path $codecovCmd
    if (!$codecov) {
        write-host "install codecov tools" -ForegroundColor Cyan
        choco install codecov
    }
    cmd.exe /c "$codecovCmd -f ""coverage.opencover.xml"""
}
else {
    write-warning "UnitTest has been skipped because current branch is ""$($env:APPVEYOR_REPO_BRANCH)"""
}