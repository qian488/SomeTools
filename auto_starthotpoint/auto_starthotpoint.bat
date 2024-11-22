@echo off
chcp 65001 > nul

:: 设置日志文件的完整路径
set "LOGFILE=%~dp0log.txt"

:: 清空日志文件
echo. > "%LOGFILE%"

:: 检查管理员权限
>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if %errorlevel% neq 0 (
    echo [%date% %time%] 需要管理员权限运行，正在请求提升... > "%LOGFILE%"
    powershell -Command "Start-Process '%~dpnx0' -Verb RunAs -WindowStyle Hidden" >nul 2>&1
    exit
)

:: 检查WLAN服务状态
echo [%date% %time%] 检查WLAN服务状态... >> "%LOGFILE%"
sc query WlanSvc >> "%LOGFILE%" 2>&1

:: 使用PowerShell启动移动热点
echo [%date% %time%] 尝试启动Windows移动热点... >> "%LOGFILE%"
powershell -Command "$connectionProfile = [Windows.Networking.Connectivity.NetworkInformation,Windows.Networking.Connectivity,ContentType=WindowsRuntime]::GetInternetConnectionProfile(); $tetheringManager = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]::CreateFromConnectionProfile($connectionProfile); try { $tetheringManager.StartTetheringAsync() } catch { $_.Exception.Message } " >> "%LOGFILE%" 2>&1

:: 等待热点启动
timeout /t 5 > nul

:: 检查热点状态
echo [%date% %time%] 检查热点状态... >> "%LOGFILE%"
powershell -Command "$connectionProfile = [Windows.Networking.Connectivity.NetworkInformation,Windows.Networking.Connectivity,ContentType=WindowsRuntime]::GetInternetConnectionProfile(); $tetheringManager = [Windows.Networking.NetworkOperators.NetworkOperatorTetheringManager,Windows.Networking.NetworkOperators,ContentType=WindowsRuntime]::CreateFromConnectionProfile($connectionProfile); $tetheringManager.TetheringOperationalState" >> "%LOGFILE%" 2>&1

echo [%date% %time%] 操作完成。 >> "%LOGFILE%"