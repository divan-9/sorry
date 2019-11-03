
PACKAGE_VERSION="$TRAVIS_TAG"

if [[ $PACKAGE_VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+.*$ ]]; then
    dotnet pack -c Release -p:PackageVersion=$PACKAGE_VERSION
    sed "s/GITHUB_TOKEN/$GITHUB_TOKEN/g" nuget.config.template > nuget.config
    dotnet nuget push ./src/Sorry.Analyzers/bin/Release/Sorry.Analyzers.$PACKAGE_VERSION.nupkg --source=GPR
else
    echo "Requested version is invalid: [$PACKAGE_VERSION]. Publishing nuget is skipped"
fi

