#r "build/FAKE/tools/FakeLib.dll"
#load "build/FAKEX/tools/fakex.fsx"

open Fake
open Fakex

RunTargetOrDefault "Build"