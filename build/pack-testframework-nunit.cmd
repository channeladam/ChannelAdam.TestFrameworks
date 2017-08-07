SET msbuild="C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\msbuild.exe"

%msbuild% ..\src\ChannelAdam.TestFramework.NUnit\ChannelAdam.TestFramework.NUnit.csproj /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v4.0;OutDir=bin\Release\net40
%msbuild% ..\src\ChannelAdam.TestFramework.NUnit\ChannelAdam.TestFramework.NUnit.csproj /t:Rebuild /p:Configuration=Release;TargetFrameworkVersion=v4.5;OutDir=bin\Release\net45

..\tools\nuget\nuget.exe pack ..\src\ChannelAdam.TestFramework.NUnit\ChannelAdam.TestFramework.NUnit.nuspec -Symbols

pause
