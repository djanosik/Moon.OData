$artifactsDir = "$PSScriptRoot\artifacts"
$version = if ($env:APPVEYOR) { $env:APPVEYOR_BUILD_VERSION } else { "1.0.0-pre" }

Exit-Build {
   move Common.props.backup Common.props -Force
}

task Clean {
    if (Test-Path $artifactsDir) { del $artifactsDir -Recurse -Force }
    del * -Include bin, obj -Recurse -Force
}

task UpdateVersion Clean, {
   copy Common.props Common.props.backup -Force
   (gc Common.props).replace("1.0.0-ci", $version) | sc Common.props
}

task RestoreDependencies UpdateVersion, {
   exec { dotnet restore }
}

task BuildSolution RestoreDependencies, {
    dir *.sln | %{
        exec { dotnet build $_.FullName -c Release }
    }
}

task RunTests BuildSolution, {
    if (Test-Path test) {
       dir test -Include *.csproj -Recurse | %{
           exec { dotnet test $_.FullName -c Release --no-build }
       }
    }
}

task PackSolution RunTests, {
    if (Test-Path src) {
        dir src -Include *.csproj -Recurse | %{
            exec { dotnet pack $_.FullName -c Release -o $artifactsDir --no-build }
        }
    }
}

task . PackSolution