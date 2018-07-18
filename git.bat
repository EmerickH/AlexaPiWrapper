echo off
cls

call defines.bat

cd %documentpath%
echo Working dir: %cd%
echo Starting cloning %origin% using Git
git clone %origin%
echo Cloning finished!