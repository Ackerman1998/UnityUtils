@echo off
for %%i in (*.proto) do (
    protogen --csharp_out=./gen/ %%i
    rem ���������¶���ע�ͣ��ɺ���
    echo From %%i To %%~ni.cs Successfully!  
)
pause