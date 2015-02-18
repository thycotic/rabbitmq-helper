# DO NOT RUN THIS MORE THAN ONCE OR YOUR PATH WILL GET LONG;LONG;LONG;LONG;LONG

$p = [Environment]::GetEnvironmentVariable("PSModulePath")
$p += ";$PSScriptRoot"
[Environment]::SetEnvironmentVariable("PSModulePath",$p)

Write-Host "New PSModulePath is $p"