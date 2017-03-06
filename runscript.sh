#! /bin/bash
if [[ "$OSTYPE" == "msys" ]]; then
    msbuild RepairHist_mmo/RepairHist_mmo.csproj
    msbuild TestClientROry/TestClientRory.csproj
else
    xbuild RepairHist_mmo/RepairHist_mmo.csproj
    xbuild TestClientROry/TestClientRory.csproj
fi
if [[ "$OSTYPE" == "darwin"* ]]; then
    mono RepairHist_mmo/bin/Debug/hist_mmorpg.exe &
    mono TestClientROry/bin/Debug/TestClientROry.exe
else
    ./RepairHist_mmo/bin/Debug/hist_mmorpg.exe &
    ./TestClientROry/bin/Debug/TestClientROry.exe
fi