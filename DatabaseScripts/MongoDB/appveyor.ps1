# init mongodb data
$initFolder = "$($env:appveyor_build_folder)\DatabaseScripts\MongoDB"
cd $initFolder
$cmd = "C:\mongodb\bin\mongo $initFolder\init.js"
iex "& $cmd"

$cmd = 'C:\mongodb\bin\mongo BootstrapAdmin --eval "printjson(db.getCollectionNames())"'
iex "& $cmd"

cd $($env:appveyor_build_folder)