@echo off
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Requesting administrator privileges...
    powershell -Command "Start-Process '%~f0' -Verb RunAs"
    exit /b
)

cd /d "%~dp0"

set PROXY_EXE=".\Proxy\bin\Debug\Proxy.exe"
set SERVER_EXE=".\Server\bin\Debug\Server.exe"
set ACTIVE_MQ_EXE=".\FakeNotification\bin\Debug\net8.0\FakeNotification.exe"
set WEB_FOLDER=".\Web"

echo Starting %PROXY_EXE%
start "PROXY" %PROXY_EXE%

echo Starting %SERVER_EXE%
start "SERVER" %SERVER_EXE%

echo Starting %ACTIVE_MQ_EXE%
start "NOTIFICATIONS" %ACTIVE_MQ_EXE%

echo Starting Python HTTP server in ./Web (port 8000)...
start "PYTHON_SERVER" python -m http.server -d %WEB_FOLDER%

echo.
echo Opening browser to http://localhost:8000 ...
start http://localhost:8000

echo.
echo All applications are running.
echo **Press Enter in THIS window to stop and close all servers.**
pause >nul

echo Stopping servers...
taskkill /F /IM Proxy.exe /T >nul 2>&1
taskkill /F /IM Server.exe /T >nul 2>&1
taskkill /F /IM FakeNotification.exe /T >nul 2>&1
taskkill /F /T /FI "WINDOWTITLE eq PYTHON_SERVER*" >nul 2>&1

echo All done.