@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

:: 设置工作目录为脚本所在目录
cd /d "%~dp0"

:: 检查管理员权限
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if %errorlevel% neq 0 (
    powershell -Command "Start-Process '%~dpnx0' -Verb RunAs -WindowStyle Hidden" >nul 2>&1
    exit
)

:: 初始化
set /a total_reconnect_time=0
set "logfile=%~dp0log.txt"
echo %date% %time% - 开始监控网络连接 > "%logfile%"

:loop
:: 检测网络连接
ping -n 1 -w 1000 bing.com >nul 2>&1
if errorlevel 1 (
    :: 网络断开，进行重连
    echo %date% %time% - 检测到网络断开，正在重连... >> "%logfile%"
    netsh interface set interface "以太网" admin=disable >nul 2>&1
    timeout /t 5 >nul
    netsh interface set interface "以太网" admin=enable >nul 2>&1
    timeout /t 15 >nul
    
    :: 记录重连时间
    set /a total_reconnect_time+=30
    echo %date% %time% - 已重连 !total_reconnect_time! 秒 >> "%logfile%"
    
    :: 检查是否超时
    if !total_reconnect_time! gtr 1800 (
        echo %date% %time% - 重连超过30分钟，程序退出 >> "%logfile%"
        goto :end
    )
) else (
    :: 网络正常
    echo %date% %time% - 网络连接正常 >> "%logfile%"
    set total_reconnect_time=0
)

timeout /t 10 >nul
goto loop

:end
exit