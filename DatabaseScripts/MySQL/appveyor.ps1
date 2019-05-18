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