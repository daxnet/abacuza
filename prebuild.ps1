#
# Substitute the version number for each AssemblyInfo.cs and project.json file
#
param (
	[string]$assemblyVersion = "1.0.0.0",
	[string]$packageVersion = "1.0.0-dev.0"
)
echo $assemblyVersion
echo $packageVersion
$files = Get-ChildItem src/services -include Build.props -Recurse
foreach ($file in $files)
{
	(Get-Content $file.FullName) |
	ForEach-Object { $_ -replace "<AbacuzaPackageVersion>0.999.0-dev</AbacuzaPackageVersion>", "<AbacuzaPackageVersion>$($packageVersion)</AbacuzaPackageVersion>" } |
	Set-Content $file.FullName

	(Get-Content $file.FullName) |
	ForEach-Object { $_ -replace "<AbacuzaAssemblyVersion>0.999.0.0</AbacuzaAssemblyVersion>", "<AbacuzaAssemblyVersion>$($assemblyVersion)</AbacuzaAssemblyVersion>" } |
	Set-Content $file.FullName

	(Get-Content $file.FullName) |
	ForEach-Object { $_ -replace "<AbacuzaAssemblyFileVersion>0.999.0.0</AbacuzaAssemblyFileVersion>", "<AbacuzaAssemblyFileVersion>$($assemblyVersion)</AbacuzaAssemblyFileVersion>" } |
	Set-Content $file.FullName
}
