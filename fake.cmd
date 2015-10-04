@echo off
cd %~dp0

SETLOCAL
SET NUGET_FOLDER=%LocalAppData%\NuGet
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto getnuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %NUGET_FOLDER% md %NUGET_FOLDER%
@powershell -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:getnuget
IF EXIST build\NuGet.exe goto getfake
IF NOT EXIST build md build
copy %CACHED_NUGET% build\NuGet.exe > nul

:getfake
IF EXIST build\FAKEX goto run
build\NuGet.exe install FAKEX -ExcludeVersion -o build

:run
build\FAKE\tools\Fake.exe fake.fsx %*