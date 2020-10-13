#
# Substitute the version number for each AssemblyInfo.cs and project.json file
#
param (
	[string]$version = "1.0.0"
)

$files = Get-ChildItem . -include *.csproj,AssemblyInfo.cs -Recurse
foreach ($file in $files)
{
	(Get-Content $file.FullName) |
	ForEach-Object { $_ -replace "<AssemblyVersion>0.999.0</AssemblyVersion>", "<AssemblyVersion>$($version)</AssemblyVersion>" } |
	Set-Content $file.FullName

	(Get-Content $file.FullName) |
	ForEach-Object { $_ -replace "<FileVersion>0.999.0</FileVersion>", "<FileVersion>$($version)</FileVersion>" } |
	Set-Content $file.FullName
}
