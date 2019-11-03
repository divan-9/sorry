rm -r _packages
mkdir _packages
mkdir _packagesSource
dotnet pack ../../sorry.sln -c Release -p:PackageVersion=0.0.0-local
dotnet nuget push ../../src/Sorry.Analyzers/bin/Release/Sorry.Analyzers.0.0.0-local.nupkg --source=local
dotnet build --no-incremental