# init mysql database
$startPath = $args[0]
if ($startPath -eq $null) {
    $startPath = "Z:\src\Longbow\BootstrapAdmin\db\SqlServer"
}

mysql  -e "drop database if exists BootstrapAdmin; create database BootstrapAdmin;" -uroot
mysql -hlocalhost -uroot -DBootstrapAdmin < $startPath\install.sql
mysql -hlocalhost -uroot -DBootstrapAdmin < $startPath\initData.sql
