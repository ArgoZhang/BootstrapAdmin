Option Explicit

Dim fso, shell
Dim targetDir, targetName, extName, keyFile, destDir, solutionDir, cmd, file, assemblyDir, outDir, batFile, signCmd
Dim msg(), m

keyFile = "..\Keys\Longbow.Utility.snk"
destDir = "Publish\Web-App\WebConsole"
assemblyDir = "C:\Longbow.Utility 2005\Release"
batFile = "..\Scripts\LgbSign.bat"
signCmd = "%ProgramFiles(x86)%\Microsoft SDKs\Windows\v7.0A\bin\sn.exe"

Set shell = WScript.CreateObject("WScript.Shell")
Set fso = CreateObject("Scripting.FileSystemObject")

If WScript.Arguments.Count > 0 Then
    targetDir = WScript.Arguments(0)
End If

If WScript.Arguments.Count > 1 Then
    targetName = WScript.Arguments(1)
End If

If WScript.Arguments.Count > 2 Then
    extName = WScript.Arguments(2)
End If

If WScript.Arguments.Count > 3 Then
    solutionDir = WScript.Arguments(3)
    keyFile = solutionDir & keyFile
    destDir = solutionDir & destDir
    batFile = solutionDir & batFile
End If

If WScript.Arguments.Count > 4 Then
    outDir = WScript.Arguments(4)
End If

If WScript.Arguments.Count > 5 Then
    If WScript.Arguments(5) = "Debug" Then
        ReSignFile targetDir & targetName & extName
        WScript.Echo WScript.Arguments(5) & " Mode... Quit copy AFTER Re-signed assembly"
        WScript.Quit
    Else 
        If extName = ".dll" Then
            WScript.Echo WScript.Arguments(5) & " Mode... Quit copy BEFORE Re-signed assembly"
            WScript.Quit
     End If
    End If
End If

destDir = destdir & targetName & "\"
If NOT fso.FolderExists(destDir) then
    fso.CreateFolder(destDir)
End If

If NOT fso.FolderExists(destdir & "Original\") then
    fso.CreateFolder(destdir & "Original\")
End If

If extName = ".exe" Then
    CopyFile fso, targetDir, destDir, targetName & extName & ".config", true
End If
CopyFile fso, targetDir, destdir & "Original\", targetName & extName, true

ReDim msg(0)
msg(0) = """" & batFile & """ """ & destdir & targetName & extName & """ """ & keyFile

For Each file in fso.GetFolder(targetDir).Files
    If fso.GetExtensionName(file) = "dll" then
        If NOT CopyFile(fso, assemblyDir, destDir, file.Name, false) Then
            ReDim Preserve msg(UBound(msg)+1)
            msg(UBound(msg)) = """" & signCmd & """ """ & destdir & file.Name & """"
            CopyFile fso, solutionDir & GetFileName(file.Name) & "\" & outDir, destDir & "Original\", file.Name, true
            ReSignFile targetdir & file.Name
        Else
            CopyFile fso, assemblyDir, destDir & "Original\", file.Name, true
        End If
    End If
Next

ReSignFile targetDir & targetName & extName
Wscript.Echo "Ready for DotFuscator... After Dotfuscator run the following command Please!"

For Each m in msg
    Wscript.Echo m
Next

Function CopyFile(fso, targetDir, destDir, targetName, show)
    If fso.FileExists(targetDir & targetName) then
        fso.CopyFile targetDir & targetName, destDir & targetName, True
        Wscript.Echo targetDir & targetName & " --> " & destDir & targetName & " Copied!"
        CopyFile = True 
    Else
        If show then
            Wscript.Echo "Missing " & targetDir & targetName & " Not Copied!"
        end if
        CopyFile = False
    End If
End Function

Function GetFileName(fileName)
    Dim pos, index
    index = 1
    Do 
        pos = index + 1
        index = InStr(pos, fileName, ".")
    Loop while index > 0

    If pos > 0 Then
        GetFileName = Left(fileName, pos -2)
    Else
        GetFileName = fileName
    End If
End Function

Sub ReSignFile(fileName)
    cmd = """"& signCmd &""" -R """ & fileName & """ " & keyFile
    shell.run cmd, 0, True
    WScript.Echo "Assembly '" & fileName & "' successfully re-signed"
End Sub