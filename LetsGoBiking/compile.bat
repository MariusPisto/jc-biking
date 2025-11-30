@echo off
setlocal enabledelayedexpansion

:: ========================================================
:: CONFIGURATION
:: ========================================================
set "JAVA_PROJECT_FOLDER=HeavyClient"

echo ========================================================
echo   Initializing Visual Studio Developer Environment
echo ========================================================

:: 1. Find Visual Studio
for /f "usebackq tokens=*" %%i in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -find Common7\Tools\VsDevCmd.bat`) do (
    set VS_DEV_CMD="%%i"
)

if not defined VS_DEV_CMD (
    echo [ERROR] Could not find Visual Studio.
    pause
    exit /b 1
)

:: 2. Load VS Environment
call %VS_DEV_CMD% -arch=x64 -host_arch=x64 -no_logo

echo.
echo ========================================================
echo   STEP 1: Cleaning Old Files
echo ========================================================

msbuild "LetsGoBiking.sln" /t:Clean
if exist "packages" (
    echo Removing old packages folder to force fresh download...
    rmdir /s /q "packages"
)

echo.
echo ========================================================
echo   STEP 2: Restoring Packages
echo ========================================================

msbuild "LetsGoBiking.sln" /t:Restore /p:RestorePackagesConfig=true /p:Configuration=Release

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Restore failed. 
    echo If this persists, open the Solution in VS and click "Restore NuGet Packages".
    pause
    exit /b 1
)

echo.
echo ========================================================
echo   STEP 3: Building Solution
echo ========================================================

msbuild "LetsGoBiking.sln" /t:Build /p:Configuration=Release

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [FAILURE] Build failed.
    pause
    exit /b 1
)

echo.
echo ========================================================
echo   STEP 4: Deploying .env to Server
echo ========================================================

set "SOURCE_ENV=.env"
set "TARGET_ROOT=Server\bin\Release"

if not exist "%SOURCE_ENV%" (
    echo [WARNING] .env file not found in current directory!
) else (
    copy /y "%SOURCE_ENV%" "%TARGET_ROOT%\" >nul
    for /d %%s in ("%TARGET_ROOT%\*") do (
        copy /y "%SOURCE_ENV%" "%%s\" >nul
    )
    echo [SUCCESS] .env copied.
)

echo.
echo ========================================================
echo   STEP 5: Building HeavyClient (Maven)
echo ========================================================

where mvn >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    :: REMOVED PARENTHESES HERE TO FIX THE CRASH
    echo [ERROR] Maven 'mvn' is not in your PATH. Skipping Java build.
) else (
    if exist "%JAVA_PROJECT_FOLDER%\pom.xml" (
        echo Found pom.xml in %JAVA_PROJECT_FOLDER%
        echo Compiling Java project...
        
        pushd "%JAVA_PROJECT_FOLDER%"
        call mvn clean package -DskipTests
        popd
        
        if !ERRORLEVEL! EQU 0 (
            echo [SUCCESS] HeavyClient Build finished.
        ) else (
            echo [FAILURE] HeavyClient Build failed.
        )
    ) else (
        echo [WARNING] Could not find pom.xml in %JAVA_PROJECT_FOLDER%.
    )
)

echo.
echo [DONE] SUCCESS!
echo Tips: run run_servers_rls.bat to start the project
pause