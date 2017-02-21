$artifactsDir = "$PSScriptRoot\artifacts"
$version = if ($env:APPVEYOR) { $env:APPVEYOR_BUILD_VERSION } else { "1.0.0-pre" }

task Clean {
    if (Test-Path $artifactsDir) { del $artifactsDir -Recurse -Force }
    del * -Include bin, obj -Recurse -Force
}

task RestoreDependencies Clean, {
   exec { dotnet restore /p:Version=$version }
}

task BuildSolution RestoreDependencies, {
    dir *.sln | %{
        exec { dotnet build $_.FullName -c Release /p:Version=$version }
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
            exec { dotnet pack $_.FullName -c Release -o $artifactsDir --no-build /p:Version=$version }
        }
    }
}

task . PackSolution