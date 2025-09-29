#!/bin/bash

# Clean All - Remove build artifacts, caches, and temporary files
# Usage: ./scripts/clean-all.sh

set -e  # Exit on any error

echo "🧹 Starting comprehensive cleanup..."

# Get the root directory (where the script is located)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

echo "📁 Working directory: $ROOT_DIR"

# Change to root directory
cd "$ROOT_DIR"

echo ""
echo "🗑️  Cleaning .NET build artifacts..."

# Remove bin and obj directories
echo "   • Removing bin/ directories..."
find . -type d -name "bin" -exec rm -rf {} + 2>/dev/null || true

echo "   • Removing obj/ directories..."
find . -type d -name "obj" -exec rm -rf {} + 2>/dev/null || true

echo ""
echo "🧽 Cleaning NuGet caches..."

# Clear NuGet caches
echo "   • Clearing NuGet HTTP cache..."
dotnet nuget locals http-cache --clear

echo "   • Clearing NuGet global packages cache..."
dotnet nuget locals global-packages --clear

echo "   • Clearing NuGet temp cache..."
dotnet nuget locals temp --clear

echo "   • Clearing NuGet plugins cache..."
dotnet nuget locals plugins-cache --clear

echo ""
echo "🔧 Cleaning .NET temp files..."

# Clean dotnet temporary files
echo "   • Running dotnet clean..."
dotnet clean --verbosity quiet

echo ""
echo "🎯 Cleaning IDE cache files..."

# Remove Rider cache (if exists)
if [ -d ".idea" ]; then
    echo "   • Removing JetBrains Rider cache (.idea/)..."
    rm -rf .idea
fi

# Remove VS Code cache (keep settings but remove cache)
if [ -d ".vscode" ]; then
    echo "   • Cleaning VS Code cache..."
    find .vscode -name "*.log" -delete 2>/dev/null || true
fi

# Remove Visual Studio cache
if [ -d ".vs" ]; then
    echo "   • Removing Visual Studio cache (.vs/)..."
    rm -rf .vs
fi

echo ""
echo "🧼 Cleaning temporary and log files..."

# Remove log files
echo "   • Removing log files..."
find . -name "*.log" -type f -delete 2>/dev/null || true

# Remove temporary files
echo "   • Removing temporary files..."
find . -name "*.tmp" -type f -delete 2>/dev/null || true
find . -name "*.temp" -type f -delete 2>/dev/null || true

# Remove macOS files
echo "   • Removing macOS system files..."
find . -name ".DS_Store" -type f -delete 2>/dev/null || true

# Remove Windows files
echo "   • Removing Windows system files..."
find . -name "Thumbs.db" -type f -delete 2>/dev/null || true

echo ""
echo "🔄 Restoring packages..."
dotnet restore --verbosity quiet

echo ""
echo "✅ Cleanup completed successfully!"
echo ""
echo "💡 Next steps:"
echo "   • Run: dotnet build"
echo "   • Or run: dotnet run --project src/WebApi"
echo ""