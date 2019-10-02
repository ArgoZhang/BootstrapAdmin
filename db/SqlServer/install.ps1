# init sqlserver database
$startPath = $args[0]
if ($startPath -eq $null) {
    $startPath = "Z:\src\Longbow\BootstrapAdmin\db\SqlServer"
}

$sqlInstance = "localhost"
$outFile = join-path $startPath "output.log"
$sqlFile = join-path $startPath "Install.sql"
$initFile = join-path $startPath "InitData.sql"

sqlcmd -S "$sqlInstance" -U sa -P sa -i "$sqlFile" -i "$initFile" -o "$outFile"