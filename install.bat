@echo off
setlocal EnableDelayedExpansion
title Installation de CornerAwake

echo ========================================================
echo               Installation de CornerAwake
echo ========================================================
echo.

set "TARGET_DIR=%LOCALAPPDATA%\CornerAwake"
set "EXE_NAME=CornerAwake.exe"
set "DESKTOP_DIR=%USERPROFILE%\Desktop"
set "SHORTCUT_PATH=%DESKTOP_DIR%\CornerAwake.lnk"

if not exist "%~dp0%EXE_NAME%" (
    echo [ERREUR] Le fichier %EXE_NAME% est introuvable dans ce dossier.
    echo Veuillez executer build.bat ou vous assurer que %EXE_NAME% est present.
    echo.
    pause
    exit /b 1
)

echo [1/3] Creation du dossier d'installation : %TARGET_DIR%...
if not exist "%TARGET_DIR%" mkdir "%TARGET_DIR%"

echo [2/3] Copie de l'executable...
taskkill /F /IM "%EXE_NAME%" >nul 2>&1
copy /Y "%~dp0%EXE_NAME%" "%TARGET_DIR%\%EXE_NAME%" >nul

echo [3/3] Creation du raccourci sur le Bureau...
powershell -NoProfile -ExecutionPolicy Bypass -Command "$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut('%SHORTCUT_PATH%'); $s.TargetPath = '%TARGET_DIR%\%EXE_NAME%'; $s.WorkingDirectory = '%TARGET_DIR%'; $s.Description = 'CornerAwake - Anti-veille coin d ecran'; $s.Save()"

echo.
echo ========================================================
echo           Installation terminee avec succes !
echo ========================================================
echo.
echo CornerAwake est installe dans :
echo %TARGET_DIR%
echo.
echo Un raccourci a ete ajoute sur votre Bureau.
echo Vous pouvez configurer le lancement avec Windows
echo via un simple clic droit sur l'icone pres de l'horloge.
echo.

set /p START_NOW="Voulez-vous lancer CornerAwake maintenant ? (O/n) : "
if /i not "%START_NOW%"=="n" (
    start "" "%TARGET_DIR%\%EXE_NAME%"
    echo Lancement effectue ! Verifiez pres de l'horloge Windows.
)

echo.
pause
