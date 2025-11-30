@echo off
setlocal enabledelayedexpansion

:: 1. Admin Rights Check
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Requesting administrator privileges...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)

:: 2. Set Root Directory
cd /d "%~dp0"

:: ========================================================
:: CONFIGURATION
:: ========================================================
set "JAVA_FOLDER=%~dp0HeavyClient\target"
set "PROXY_EXE=.\Proxy\bin\Release\Proxy.exe"
set "SERVER_EXE=.\Server\bin\Release\Server.exe"
set "ACTIVE_MQ_EXE=.\FakeNotification\bin\Release\net8.0\FakeNotification.exe"
set "WEB_FOLDER=.\Web"

echo ========================================================
echo   Starting Applications...
echo ========================================================

echo [OK] Starting PROXY...
start "PROXY" "!PROXY_EXE!"

echo [OK] Starting SERVER...
start "SERVER" "!SERVER_EXE!"

echo [OK] Starting NOTIFICATIONS...
start "NOTIFICATIONS" "!ACTIVE_MQ_EXE!"

:: ---------------------------------------------------------
:: JAVA CLIENT (Auto-find JAR)
:: ---------------------------------------------------------
set "JAVA_JAR_NAME="
if exist "%JAVA_FOLDER%" (
    pushd "%JAVA_FOLDER%"
    for /f "delims=" %%f in ('dir /b /a-d *.jar 2^>nul ^| findstr /v "^original-"') do (
        set "JAVA_JAR_NAME=%%f"
        goto :FoundJar
    )
    popd
)

:FoundJar
popd 2>nul
:: We assume we found it.
echo [OK] Starting HEAVY CLIENT: !JAVA_JAR_NAME!
start "HEAVY_CLIENT_APP" /D "%JAVA_FOLDER%" cmd /k "java -jar !JAVA_JAR_NAME!"

:: ---------------------------------------------------------

echo [OK] Starting Python HTTP server...
start "PYTHON_SERVER" python -m http.server -d %WEB_FOLDER%

echo.
echo Opening browser...
start http://localhost:8000

echo.
echo ========================================================
echo   ALL SYSTEMS RUNNING
echo   Press Enter in THIS window to stop and close all servers.
echo ========================================================
pause >nul

echo Stopping servers...
taskkill /F /IM Proxy.exe /T >nul 2>&1
taskkill /F /IM Server.exe /T >nul 2>&1
taskkill /F /IM FakeNotification.exe /T >nul 2>&1
taskkill /F /T /FI "WINDOWTITLE eq PYTHON_SERVER*" >nul 2>&1
taskkill /F /T /FI "WINDOWTITLE eq HEAVY_CLIENT_APP*" >nul 2>&1

echo All done.