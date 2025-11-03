@echo off
rem run_servers_dbg.bat.bat - runs exe one after another, waits for each to finish

net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Requesting administrator privileges...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)

cd /d "%~dp0"

set PROXY=".\Proxy\bin\Debug\Proxy.exe"
set SERVER=".\Serveur\bin\Debug\Serveur.exe"

echo Starting %PROXY%
start "PROXY" %PROXY%
echo Exit code: %ERRORLEVEL%

echo Starting %SERVER%
start "SERVER" %SERVER%
echo Exit code: %ERRORLEVEL%

echo All done.