@echo off
title Desinstallation de CornerAwake

echo ========================================================
echo             Desinstallation de CornerAwake
echo ========================================================
echo.

set "TARGET_DIR=%LOCALAPPDATA%\CornerAwake"
set "EXE_NAME=CornerAwake.exe"
set "DESKTOP_DIR=%USERPROFILE%\Desktop"
set "SHORTCUT_PATH=%DESKTOP_DIR%\CornerAwake.lnk"

echo [1/4] Fermeture de l'application si elle est en cours d'execution...
taskkill /F /IM "%EXE_NAME%" >nul 2>&1

echo [2/4] Suppression du demarrage automatique Windows...
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v "CornerAwake" /f >nul 2>&1

echo [3/4] Suppression du raccourci Bureau...
if exist "%SHORTCUT_PATH%" del /F /Q "%SHORTCUT_PATH%" >nul 2>&1

echo [4/4] Suppression des fichiers installes...
if exist "%TARGET_DIR%" rmdir /S /Q "%TARGET_DIR%" >nul 2>&1

echo.
echo CornerAwake a ete completement supprime de votre ordinateur.
echo.
pause
