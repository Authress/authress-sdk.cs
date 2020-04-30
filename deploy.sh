#!/usr/bin/env bash

dotnet test -c Release || exit 1

branchId="${TRAVIS_BRANCH/release\//}"
buildNumber="${branchId}.${TRAVIS_BUILD_NUMBER}"
sed "s/0.0.1/${buildNumber}/g" src/Authress.SDK/*.csproj -i
dotnet pack -c Release -o ../../artifacts || exit 1
dotnet nuget push artifacts/Authress.SDK.${buildNumber}.nupkg -s "https://api.nuget.org/v3/index.json" -k "$NUGET_API_KEY"
