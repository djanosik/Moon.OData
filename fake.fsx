#r "build/FAKE/tools/FakeLib.dll"
#load "build/FAKEX/tools/fakex.fsx"
open Fake

Target "SetupRuntime" (fun _ ->
    if (environVar "SKIP_DNX_INSTALL") <> "1" then
        dnvm "install '1.0.0-beta7' -a default -runtime CLR -arch x86 -nonative"
        dnvm "install default -runtime CoreCLR -arch x86 -nonative"
        
    dnvm "use default -runtime CLR -arch x86"
)

"SetupRuntime" ==> "RestoreDependencies"

RunTargetOrDefault "Build"