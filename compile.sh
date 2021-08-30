# --self-contained true for compiling the runtime into the project
# -r for system+architecture
# project file = ../Scratch-Bot-core/Scratch-Bot-core.csproj
# -o for output directory
dotnet publish --self-contained true -r linux-arm64 Scratch-Bot-core/Scratch-Bot-core.csproj -o build/
