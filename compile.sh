#!/bin/bash
set -e
# --self-contained true for compiling the runtime into the project
# -r for os+architecture
# project file = Scratch-Bot-core/Scratch-Bot-core.csproj
# -o for output directory
#dotnet publish --self-contained true -r linux-arm64 Scratch-Bot-core/Scratch-Bot-core.csproj -o build/
echo "cleaning old stuff"
if [ -d ./build ]; then
    rm -r ./build
fi

echo "compiling and testing"
dotnet publish && dotnet test

if [ ! -d ./build ]; then
    mkdir build
fi

echo "copy build to dir"
cp -r App/bin/Release/net8.0/publish/* build/

echo "copy service file"
sudo cp Scratch-Bot.service /etc/systemd/system/Scratch-Bot.service
