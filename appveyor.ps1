if ("$($env:APPVEYOR_REPO_BRANCH)" -eq "master")
{
    echo "" "Install coveralls.net tools"

    dotnet tool install coveralls.net --version 1.0.0 --tool-path "./tools"
  
    # init sqlserver database
    $startPath = "$($env:appveyor_build_folder)\DatabaseScripts"
    $sqlInstance = "(local)\SQL2014"
    $outFile = "$($env:appveyor_build_folder)\DatabaseScripts\output.log"
    $sqlFile = join-path $startPath "Install.sql"
    $initFile = join-path $startPath "InitData.sql"

    sqlcmd -S "$sqlInstance" -U sa -P Password12! -i "$sqlFile" -i "$initFile" -o "$outFile"

    # init mysql database
    $env:MYSQL_PWD="Password12!"
    $cmd = '"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe" -e "create database BootstrapAdmin;" -uroot'
    cmd.exe /c $cmd

    $sqlFile = join-path $startPath "MySQL\Install.sql"
    $cmd = '"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe" -hlocalhost -uroot -DBootstrapAdmin <'
    cmd.exe /c $cmd $sqlFile

    $initFile = join-path $startPath "MySQL\InitData.sql"
    $cmd = '"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe" -hlocalhost -uroot -DBootstrapAdmin <'
    cmd.exe /c $cmd $initFile

    # init mongodb data
    $initFolder = join-path $startPath "MongoDB"
    cd $initFolder
    $cmd = "C:\mongodb\bin\mongo $initFolder\init.js"
    iex "& $cmd"

    $cmd = 'C:\mongodb\bin\mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"'
    iex "& $cmd"

    cd $($env:appveyor_build_folder)
    dotnet test UnitTest --no-restore /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Include="[Bootstrap*]*" /p:CoverletOutput=../
    cmd.exe /c ".\tools\csmacnz.Coveralls.exe --opencover -i coverage.opencover.xml --useRelativePaths"
}