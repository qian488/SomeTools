@echo off
chcp 65001 > nul
if exist log.txt (
    type nul > log.txt
)
exit