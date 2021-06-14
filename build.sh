#!/bin/bash
set -x

if [[ $(git status -s) ]]; then
    echo "You have uncommitted files. Commit and push them before running this script."
  #  exit 1
fi

git fetch --tags

# get latest git tag and increase by one (see https://stackoverflow.com/questions/4485399/how-can-i-bump-a-version-number-using-bash)
VERSION=`git describe --abbrev=0 | awk -F. '/[0-9]+\./{$NF+=1;OFS=".";print}' | sed 's/ /./g'`

echo "setting version to $VERSION"

XAMARIN_TOOLS=/Library/Frameworks/Mono.framework/Versions/Current/Commands/
NUGET="$XAMARIN_TOOLS/nuget"

function setTag {
  git tag -a $VERSION -m ''  || exit 1
  git push --tags || exit 1
}

function publishNuGet {
  nuget push $1 -Source https://www.nuget.org/api/v2/package || exit 1
}

$NUGET restore SimpleLocation.sln || exit 1

msbuild /p:Configuration=Release Droid/Droid.csproj || exit 1
msbuild /p:Configuration=Release iOS/iOS.csproj || exit 1
msbuild Net/Net.csproj || exit 1

sed -i '' "s/\(<version>\).*\(<\/version>\)/\1$VERSION\2/" SimpleLocation.nuspec
$NUGET pack SimpleLocation.nuspec || exit 1

# setTag

if [[ $PUBLISH_NUGET == True ]]; then
  publishNuGet SimpleLocation.*.nupkg
fi
