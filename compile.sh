#!/bin/bash
clear
set -e
# --self-contained true for compiling the runtime into the project
# -r for os+architecture
# project file = Scratch-Bot-core/Scratch-Bot-core.csproj
# -o for output directory
#dotnet publish --self-contained true -r linux-arm64 Scratch-Bot-core/Scratch-Bot-core.csproj -o build/
arch=arm64
#arch=x64

outputDir=./build
publishDir=./App/bin/Release/net8.0/linux-$arch
servicePath=/etc/systemd/system
serviceFilename=scratchBot.service
tknFile=token.tkn

echo "cleaning old stuff"
if [ -d $outputDir ]; then
    rm -r $outputDir
fi

echo "compiling and testing"
#dotnet test && dotnet publish --os linux --arch arm64 --self-contained
dotnet test && dotnet publish -c Release


if [ ! -d $outputDir ]; then
    mkdir build
fi

echo "copy build to dir from '$publishDir'"
cp -r $publishDir/* $outputDir/

echo "copy tkn file"
sudo cp ~/$tknFile /

echo "copy service file"
sudo cp $serviceFilename $servicePath/$serviceFilename
