use 4.0 MSBuild

task Clean {
    del * -Include artifacts, bin, obj -Recurse -Force
}

task BackupProjects Clean, {
    dir * -Include project.json, project.lock.json -Recurse |% {
        copy $_.FullName ($_.FullName + ".bak") -Force
    }
}

task UpdateVersions BackupProjects, {
    $version = if($env:APPVEYOR) { $env:APPVEYOR_BUILD_VERSION } else { "1.0.0" }

    dir * -Include project.json, project.lock.json -Recurse |% {
        (gc $_.FullName) |% { $_ -replace "1.0.0-ci", $version } | sc $_.FullName
    }
}

task RestoreDependencies UpdateVersions, {
   exec { dotnet restore }
}

task PackProjects RestoreDependencies, {
    if(Test-Path src) {
        dir src -Include project.json -Recurse |% {
            $project = $_.FullName
            exec { dotnet pack $project -c Release }
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
        dir test -Include project.json -Recurse |% {
            $project = $_.FullName
            exec { dotnet test $project -c Release }
        }
    }
}

task RestoreProjects RunTests, {
    dir * -Include project.json.bak, project.lock.json.bak -Recurse |% {
        move $_.FullName $_.FullName.Replace(".bak", "") -Force
    }
}

task . RestoreProjects