@echo off
for %%i in (*.proto) do (
    protogen --csharp_out=./gen/ %%i
    rem 从这里往下都是注释，可忽略
    echo From %%i To %%~ni.cs Successfully!  
)
pause