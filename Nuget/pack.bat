@Echo Setting up folder structure
md Package\lib\net45\

@Echo Removing old files
del /Q Package\lib\net45\*.*

@Echo Copying new files
copy ..\VersionChecker\bin\Release\VersionChecker.dll Package\lib\net45

@Echo Packing files
"..\Solution File\.nuget\nuget.exe" pack package\VersionChecker.nuspec