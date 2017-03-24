#! /bin/bash
if [[ "$OSTYPE" == "msys" ]]; then
    msbuild GtkClient/GtkClient/GtkClient.sln
else
    xbuild GtkClient/GtkClient/GtkClient.sln
fi
if [[ "$OSTYPE" == "darwin"* ]]; then
    mono RepairHist_mmo/bin/Debug/hist_mmorpg.exe &
    mono GtkClient/GtkClient/GtkClient/bin/Debug/GtkClient.exe
else
    ./RepairHist_mmo/bin/Debug/hist_mmorpg.exe &
    ./GtkClient/GtkClient/GtkClient/bin/Debug/GtkClient.exe
fi