
PACKAGE_VERSION="$TRAVIS_TAG"

if [[ $PACKAGE_VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+.*$ ]]; then
    dotnet pack -c Release -p:PackageVersion=$PACKAGE_VERSION
    dotnet nuget push ./src/Sorry.Analyzers/bin/Release/Sorry.Analyzers.$PACKAGE_VERSION.nupkg -k=$NUGET_KEY -s https://api.nuget.org/v3/index.json
else
    echo "Requested version is invalid: [$PACKAGE_VERSION]. Publishing nuget is skipped"
fi