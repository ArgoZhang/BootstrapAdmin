# init sqlserver database
$startPath = "$($env:appveyor_build_folder)\DatabaseScripts\SqlServer"
$sqlInstance = "(local)\SQL2014"
$outFile = join-path $startPath "output.log"
$sqlFile = join-path $startPath "Install.sql"
$initFile = join-path $startPath "InitData.sql"

sqlcmd -S "$sqlInstance" -U sa -P Password12! -i "$sqlFile" -i "$initFile" -o "$outFile"