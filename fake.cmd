@echo off
cd %~dp0

SETLOCAL
SET BUILD_FOLDER=build
SET NUGET=%BUILD_FOLDER%\NuGet.exe

:getnuget
IF EXIST %NUGET% GOTO getfakex
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %BUILD_FOLDER% md %BUILD_FOLDER%
@powershell -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'http://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile '%NUGET%'"

:getfakex
IF EXIST %BUILD_FOLDER%\FAKEX goto run
%NUGET% install FAKEX -Source "https://api.nuget.org/v3/index.json" -ExcludeVersion -o build

:run
%BUILD_FOLDER%\FAKE\tools\Fake.exe fake.fsx %*