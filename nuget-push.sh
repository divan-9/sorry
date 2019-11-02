dotnet pack -c Release -p:PackageVersion=$PACKAGE_VERSION
sed "s/GITHUB_TOKEN/$GITHUB_TOKEN/g" nuget.config.template > nuget.config
dotnet nuget push ./src/Sorry.Analyzers/bin/Release/Sorry.Analyzers.$PACKAGE_VERSION.nupkg --source=GPR
