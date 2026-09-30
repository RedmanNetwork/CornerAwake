@echo off
setlocal
title Compilation de CornerAwake

echo ========================================================
echo             Compilation de CornerAwake
echo ========================================================
echo.

set "CSC_PATH="
if exist "%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
    set "CSC_PATH=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
) else if exist "%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
    set "CSC_PATH=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)

if "%CSC_PATH%"=="" (
    echo [ERREUR] Compilateur C# csc.exe introuvable.
    echo Veuillez vous assurer que .NET Framework 4.5+ est installe.
    pause
    exit /b 1
)

:: Fermer CornerAwake s'il est actif pour permettre l'ecriture du .exe
taskkill /F /IM CornerAwake.exe >nul 2>&1

echo Utilisation du compilateur : %CSC_PATH%
"%CSC_PATH%" /target:winexe /optimize+ /platform:anycpu /out:CornerAwake.exe CornerAwake.cs

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCES] CornerAwake.exe a ete genere avec succes !
) else (
    echo.
    echo [ERREUR] La compilation a echoue.
)

echo.
