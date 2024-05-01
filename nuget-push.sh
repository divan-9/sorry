
PACKAGE_VERSION="$1"

if [[ $PACKAGE_VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+.*$ ]]; then
    echo "Running pack for [$PACKAGE_VERSION]..."
    dotnet pack -c Release -p:PackageVersion=$PACKAGE_VERSION

    echo "Running push for [./src/Sorry.Analyzers/bin/Release/Sorry.Analyzers.$PACKAGE_VERSION.nupkg] [$NUGET_KEY]"
    dotnet nuget push ./src/Sorry.Analyzers/bin/Release/Sorry.Analyzers.$PACKAGE_VERSION.nupkg -k=$NUGET_KEY -s https://api.nuget.org/v3/index.json
else
    echo "Requested version is invalid: [$PACKAGE_VERSION]. Publishing nuget is skipped"
fi