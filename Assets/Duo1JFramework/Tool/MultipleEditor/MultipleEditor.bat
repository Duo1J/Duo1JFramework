@echo off

setlocal

set src=
set tar=

if not exist %tar% (
	md %tar%
)

echo Source: %src%
echo Target: %tar%

pause

mklink /J %tar%\Assets %src%\Assets
mklink /J %tar%\Packages %src%\Packages
mklink /J %tar%\ProjectSettings %src%\ProjectSettings

endlocal

pause