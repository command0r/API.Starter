@echo off
REM Clean All - Remove build artifacts, caches, and temporary files
REM Usage: scripts\clean-all.bat

setlocal enabledelayedexpansion

echo 🧹 Starting comprehensive cleanup...
echo.

REM Get the directory where this script is located
set "SCRIPT_DIR=%~dp0"
set "ROOT_DIR=%SCRIPT_DIR%.."

echo 📁 Working directory: %ROOT_DIR%
echo.

REM Change to root directory
cd /d "%ROOT_DIR%"

echo 🗑️  Cleaning .NET build artifacts...

REM Remove bin and obj directories
echo    • Removing bin/ directories...
for /d /r . %%d in (bin) do (
    if exist "%%d" (
        rd /s /q "%%d" 2>nul
    )
)

echo    • Removing obj/ directories...
for /d /r . %%d in (obj) do (
    if exist "%%d" (
        rd /s /q "%%d" 2>nul
    )
)

echo.
echo 🧽 Cleaning NuGet caches...

REM Clear NuGet caches
echo    • Clearing NuGet HTTP cache...
dotnet nuget locals http-cache --clear >nul

echo    • Clearing NuGet global packages cache...
dotnet nuget locals global-packages --clear >nul

echo    • Clearing NuGet temp cache...
dotnet nuget locals temp --clear >nul

echo    • Clearing NuGet plugins cache...
dotnet nuget locals plugins-cache --clear >nul

echo.
echo 🔧 Cleaning .NET temp files...

REM Clean dotnet temporary files
echo    • Running dotnet clean...
dotnet clean --verbosity quiet >nul

echo.
echo 🎯 Cleaning IDE cache files...

REM Remove Rider cache (if exists)
if exist ".idea" (
    echo    • Removing JetBrains Rider cache (.idea/^)...
    rd /s /q ".idea" 2>nul
)

REM Remove Visual Studio cache
if exist ".vs" (
    echo    • Removing Visual Studio cache (.vs/^)...
    rd /s /q ".vs" 2>nul
)

echo.
echo 🧼 Cleaning temporary and log files...

REM Remove log files
echo    • Removing log files...
for /r . %%f in (*.log) do (
    if exist "%%f" del /q "%%f" 2>nul
)

REM Remove temporary files
echo    • Removing temporary files...
for /r . %%f in (*.tmp) do (
    if exist "%%f" del /q "%%f" 2>nul
)
for /r . %%f in (*.temp) do (
    if exist "%%f" del /q "%%f" 2>nul
)

REM Remove Windows files
echo    • Removing Windows system files...
for /r . %%f in (Thumbs.db) do (
    if exist "%%f" del /q "%%f" 2>nul
)

echo.
echo 🔄 Restoring packages...
dotnet restore --verbosity quiet

echo.
echo ✅ Cleanup completed successfully!
echo.
echo 💡 Next steps:
echo    • Run: dotnet build
echo    • Or run: dotnet run --project src/WebApi
echo.

pause