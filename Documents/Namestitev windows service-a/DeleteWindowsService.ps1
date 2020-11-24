"`n"
Write-Host "Pred začetkom namestitve je potrebno imeti installUtil.exe v istem direktoriju kot skripto in servis `n"

$ServiceFile = Read-Host "`n Vnesi naziv in koncnicno namestitvenega programa (.exe)"

Write-Host $Location

$confirm = Read-Host "`n Želite namestiti winwos service? (y/n)"

if($confirm -eq "y")
{
    Get-Service -Name $ServiceName | Stop-Service
    .\InstallUtil.exe /u $ServiceFile
}
else
{
    break
}
