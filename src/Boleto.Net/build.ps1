 $rootDir = $env:APPVEYOR_BUILD_FOLDER
 $buildNumber = $env:APPVEYOR_BUILD_NUMBER
 $solutionFile = "$rootDir\src\Boleto.Net.sln"
 $srcDir = "$rootDir\src\nuget\boleto.net"
 $slns = ls "$rootDir\src\*.sln"
 $packagesDir = "$rootDir\src\packages"
 $nuspecPath = "$rootDir\src\nuget\boleto.net\boleto.net.nuspec"
 $nugetExe = "$packagesDir\NuGet.CommandLine.2.8.5\tools\NuGet.exe"
 $nupkgPath = "$rootDir\src\NuGet\Boleto.Net\Boleto.Net.{0}.nupkg"

foreach($sln in $slns) {
   nuget restore $sln
}

[xml]$xml = cat $nuspecPath
$xml.package.metadata.version+=".$buildNumber"
$xml.Save($nuspecPath)

[xml]$xml = cat $nuspecPath
$nupkgPath = $nupkgPath -f $xml.package.metadata.version

nuget pack $nuspecPath -properties "Configuration=$env:configuration;Platform=AnyCPU;Version=$($env:appveyor_build_version)" -OutputDirectory $srcDir 
appveyor PushArtifact $nupkgPath
