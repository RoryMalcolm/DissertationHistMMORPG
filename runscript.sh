#! /bin/bash
xbuild RepairHist_mmo/RepairHist_mmo.csproj
xbuild TestClientROry/TestClientRory.csproj
./RepairHist_mmo/bin/Debug/hist_mmorpg.exe &
./TestClientROry/bin/Debug/TestClientROry.exe