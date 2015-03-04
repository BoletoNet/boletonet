 $rootDir = "."
 $solutionFileCS = "$rootDir\src\Boleto.Net.sln"
 $srcDir = "$rootDir\src"
 $isAppVeyor = $env:APPVEYOR -eq $true
 $slns = ls "$rootDir\*.sln"
 $packagesDir = "$rootDir\src\packages"
 $buildNumber = [Convert]::ToInt32($env:APPVEYOR_BUILD_NUMBER).ToString("0000")
 $nuspecPathCS = "$rootDir\src\nuget\boleto.net\boleto.net.2.0.12.nuspec"
 $nugetExe = "$packagesDir\NuGet.CommandLine.2.8.3\tools\NuGet.exe"
 $nupkgPathCS = "$rootDir\src\NuGet\Boleto.Net\Boleto.Net.{0}.nupkg"

 $logDir = "$rootDir\log"
 $isRelease = $isAppVeyor -and ($env:APPVEYOR_REPO_BRANCH -eq "release")
 $isPullRequest = $env:APPVEYOR_PULL_REQUEST_NUMBER -ne $null

 if ((Test-Path $logDir) -eq $false)
 {
     Write-Host -ForegroundColor DarkBlue "Creating log directory $logDir"
     mkdir $logDir | Out-Null
 }

 Foreach($sln in $slns) {
     RestorePkgs $sln
 }

Write-Host "Packing nuget for $language..."
[xml]$xml = cat $nuspecPathCS
$nupkgPathCS = $nupkgPathCS -f $xml.package.metadata.version
Write-Host "Nupkg path is $nupkgFile"
. $nugetExe pack $nuspecPathCS -Properties "Configuration=Debug;Platform=AnyCPU" -OutputDirectory $srcDir
ls $nupkgPathCS
Write-Host "Nuget packed for $language!"
Write-Host "Pushing nuget artifact for $language..."
appveyor PushArtifact $nupkgPathCS
Write-Host "Nupkg pushed for $language!"
