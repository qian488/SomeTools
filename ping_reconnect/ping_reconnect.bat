@echo off
setlocal enabledelayedexpansion
set /a total_reconnect_time=0

:loop
ping -n 1 -w 1000 bing.com >> log.txt 2>&1
if errorlevel 1 (
    echo No internet connection detected. Reconnecting... >> log.txt 2>&1
    netsh interface set interface name="以太网" admin=disable
    netsh interface set interface name="以太网" admin=enable
    
    set /a total_reconnect_time+=10
    echo Reconnect attempt #!total_reconnect_time! >> log.txt 2>&1
    if !total_reconnect_time! gtr 1800 (
	echo Error: Reconnect attempts exceeded 30 minutes. Giving up. >> log.txt 2>&1
	goto :end
    ) 
) else (
    echo Internet connection is stable. >> log.txt 2>&1
    set total_reconnect_time=0
)
timeout /t 10
goto loop

:end
echo Script ended. >> log.txt 2>&1