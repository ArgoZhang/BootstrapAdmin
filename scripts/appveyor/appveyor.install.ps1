# 显示 dotnet version
write-host "dotnet --version" -ForegroundColor Cyan
dotnet --version

# 注意 my.ini 文件行结束符必须为CRLF

$iniPath="C:\Program Files\MySQL\MySQL Server 5.7\my.ini"
write-host "copy $($env:appveyor_build_folder)\db\MySQL\my.ini -> $iniPath" -ForegroundColor Cyan
xcopy "$($env:appveyor_build_folder)\db\MySQL\my.ini" $iniPath /y
$newText = ([System.IO.File]::ReadAllText($iniPath)).Replace("\n", "\r\n")
[System.IO.File]::WriteAllText($iniPath, $newText)

write-host "starting database services ..." -ForegroundColor Cyan
