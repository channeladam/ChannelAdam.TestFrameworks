@echo off

set packageName=ChannelAdam.TestFramework.NUnit

set /p version="What is the version you want to push? i.e. (3.x.y)"

..\tools\nuget\nuget.exe push "%packageName%.%version%.nupkg"

pause
