
nuget install NUnit.Runners -Version 3.4.1 -OutputDirectory tools
nuget install OpenCover -Version 4.6.519 -OutputDirectory tools
nuget install coveralls.net -Version 0.7.0 -OutputDirectory tools
 
.\tools\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:.\tools\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe -targetargs:".\Alfred.Tests\bin\Release\Alfred.Tests.dll .\Alfred.Model.Implementation.Tests\bin\Release\Alfred.Model.Implementation.Tests.dll" -filter:"+[Alfred*]* -[*.Tests]* -[*]*ObjectDiffPatch*" -register:user
.\tools\coveralls.net.0.7.0\tools\csmacnz.Coveralls.exe --opencover -i .\results.xml