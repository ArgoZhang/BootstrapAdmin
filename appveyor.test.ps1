if ("$($env:APPVEYOR_REPO_BRANCH)" -eq "master")
{
    # init sqlserver database
    $startPath = "$($env:appveyor_build_folder)\DatabaseScripts\SqlServer"
    $sqlInstance = "(local)\SQL2014"
    $outFile = join-path $startPath "output.log"
    $sqlFile = join-path $startPath "Install.sql"
    $initFile = join-path $startPath "InitData.sql"

    sqlcmd -S "$sqlInstance" -U sa -P Password12! -i "$sqlFile" -i "$initFile" -o "$outFile"

    # init mysql database
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

    # init mongodb data
    $initFolder = "$($env:appveyor_build_folder)\DatabaseScripts\MongoDB"
    cd $initFolder

    cmd.exe /c "C:\mongodb\bin\mongo init.js"

    $cmd = 'C:\mongodb\bin\mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"'
    iex "& $cmd"

    cd $($env:appveyor_build_folder)

    echo "" "Install coveralls.net tools"
    dotnet tool install coveralls.net --version 1.0.0 --tool-path "./tools"

    dotnet test UnitTest --no-restore /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Include="[Bootstrap*]*" /p:ExcludeByFile="../Bootstrap.Admin/Program.cs%2c../Bootstrap.Admin/Startup.cs%2c../Bootstrap.Admin/HttpHeaderOperation.cs" /p:CoverletOutput=../
    cmd.exe /c ".\tools\csmacnz.Coveralls.exe --opencover -i coverage.opencover.xml --useRelativePaths"
}