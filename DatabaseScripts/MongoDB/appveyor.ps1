# init mongodb data
$initFolder = "$($env:appveyor_build_folder)\DatabaseScripts\MongoDB"
cd $initFolder

cmd.exe /c "C:\mongodb\bin\mongo init.js"

$cmd = 'C:\mongodb\bin\mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"'
iex "& $cmd"

cd $($env:appveyor_build_folder)