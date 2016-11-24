task Clean {
    if(Test-Path artifacts) { del artifacts -Recurse -Force }
    del * -Include bin, obj -Recurse -Force
}

task RestoreDependencies Clean, {
   exec { dotnet restore }
}

task PackProjects RestoreDependencies, {
    $version = if($env:APPVEYOR) { $env:APPVEYOR_BUILD_VERSION } else { "1.0.0-pre" }

    if(Test-Path src) {
        dir src -Include *.csproj -Recurse |% {
            $project = $_.FullName
            exec { dotnet pack $project -c Release /p:Version=$version }
        }
    }
}

task CopyArtifacts PackProjects, {
    ni "artifacts" -ItemType Directory -Force
    
    if(Test-Path src) {
        dir src -Include *.nupkg -Recurse |% {
            copy $_.FullName ("artifacts") -Force
        }
    }
}

task RunTests CopyArtifacts, {
    if(Test-Path test) {
       dir test -Include *.csproj -Recurse |% {
           $project = $_.FullName
           exec { dotnet test $project -c Release }
       }
    }
}

task . RunTests