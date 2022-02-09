#!/usr/bin/env bash

dotnet test -c Release || exit 1

branchId="${GITHUB_REF/refs\/heads\/release\//}"
buildNumber="${branchId}.${GITHUB_RUN_NUMBER}"
sed "s/0.0.1/${buildNumber}/g" src/Authress.SDK/*.csproj -i
dotnet pack -c Release -o ../../artifacts || exit 1
dotnet nuget push artifacts/Authress.SDK.${buildNumber}.nupkg -s "https://api.nuget.org/v3/index.json" -k "$NUGET_API_KEY"
