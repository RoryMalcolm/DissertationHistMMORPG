#! /bin/bash
if [[ "$OSTYPE" == "msys" ]]; then
    msbuild RepairHist_mmo/RepairHist_mmo.csproj
    msbuild TestClientROry/TestClientRory.csproj
else
    xbuild RepairHist_mmo/RepairHist_mmo.csproj
    xbuild TestClientROry/TestClientRory.csproj
fi
./RepairHist_mmo/bin/Debug/hist_mmorpg.exe &
./TestClientROry/bin/Debug/TestClientROry.exe