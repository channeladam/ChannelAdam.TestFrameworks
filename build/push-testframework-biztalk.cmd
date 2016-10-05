@echo off

set packageName=ChannelAdam.TestFramework.BizTalk

set /p version="What is the version you want to push?"

..\tools\nuget\nuget.exe push "%packageName%.%version%.nupkg"

pause
