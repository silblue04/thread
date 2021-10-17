SET BAT_PATH=%~dp0
start ./application.exe
xcopy %BAT_PATH%/projectM/Assets/Resources/Metas/*.txt %BAT_PATH%/Project/Assets/Resources/Metas/
del  %BAT_PATH%/projectM