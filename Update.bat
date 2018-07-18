echo off
cls

call defines.bat

PING localhost -n 3 >NUL

cd %alexapipath%
echo Working dir: %cd%
echo Starting updating %alexapipath% using Git
git pull
echo Updating finished!
pause