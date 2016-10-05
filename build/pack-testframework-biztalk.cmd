SET msbuild="C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"

%msbuild% ..\src\ChannelAdam.TestFramework.BizTalk\ChannelAdam.TestFramework.BizTalk.csproj /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;OutDir=bin\Release\net40

..\tools\nuget\nuget.exe pack ..\src\ChannelAdam.TestFramework.BizTalk\ChannelAdam.TestFramework.BizTalk.nuspec -Symbols

pause
