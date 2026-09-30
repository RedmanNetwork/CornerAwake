@echo off
title Desinstallation de CornerAwake

echo ========================================================
echo             Desinstallation de CornerAwake
echo ========================================================
echo.

set "TARGET_DIR=%LOCALAPPDATA%\CornerAwake"
set "EXE_NAME=CornerAwake.exe"

echo [1/4] Fermeture de l'application si elle est en cours d'execution...
taskkill /F /IM "%EXE_NAME%" >nul 2>&1

echo [2/4] Suppression du demarrage automatique Windows...
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v "CornerAwake" /f >nul 2>&1

echo [3/4] Suppression du raccourci Bureau...
powershell -NoProfile -ExecutionPolicy Bypass -Command "$ws = New-Object -ComObject WScript.Shell; $desk = $ws.SpecialFolders('Desktop'); $lnk = Join-Path $desk 'CornerAwake.lnk'; if (Test-Path $lnk) { Remove-Item $lnk -Force }"

echo [4/4] Suppression des fichiers installes...
if exist "%TARGET_DIR%" (
    :: Si execute depuis TARGET_DIR, on supprime tout sauf le script courant puis auto-delete
    timeout /t 1 /nobreak >nul
    del /F /Q "%TARGET_DIR%\%EXE_NAME%" >nul 2>&1
    del /F /Q "%TARGET_DIR%\install.bat" >nul 2>&1
)

echo.
echo CornerAwake a ete retire de votre ordinateur.
echo.
pause
