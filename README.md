﻿# Cake

Cake (C# Make) is a build automation system with a C# DSL to do things like compiling code, copy files/folders, running unit tests, compress files and build NuGet packages.

| Runner         | Latest Released | Latest Develop |
|----------------|-----------------|----------------|
| Cake .NET Tool | [![NuGet](https://img.shields.io/nuget/v/Cake.Tool.svg)](https://www.nuget.org/packages/Cake.Tool) | [![Azure Artifacts](https://azpkgsshield.azurevoodoo.net/cake-build/Cake/cake/cake.tool)](https://dev.azure.com/cake-build/Cake/_packaging?_a=package&feed=cake&package=Cake.Tool&protocolType=NuGet) |
| Cake Frosting | [![NuGet](https://img.shields.io/nuget/v/Cake.Frosting.svg)](https://www.nuget.org/packages/Cake.Frosting) | [![Azure Artifacts](https://azpkgsshield.azurevoodoo.net/cake-build/Cake/cake/Cake.Frosting)](https://dev.azure.com/cake-build/Cake/_packaging?_a=package&feed=cake&package=Cake.Frosting&protocolType=NuGet) |


## Continuous integration

| Build server                | Platform      | Build status                                                                                                                                                        | Integration tests                                                                                                                                                   |
|-----------------------------|---------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Azure Pipelines             | MacOS         | [![Azure Pipelines Mac Build status](https://dev.azure.com/cake-build/Cake/_apis/build/status/Azure%20Pipelines%20-%20Build%20Cake%20Mac?&branchName=develop)](https://dev.azure.com/cake-build/Cake/_build/latest?definitionId=4) | |
| Azure Pipelines             | Windows       | [![Azure Pipelines Windows Build status](https://dev.azure.com/cake-build/Cake/_apis/build/status/Azure%20Pipelines%20-%20Build%20Cake%20Windows?&branchName=develop)](https://dev.azure.com/cake-build/Cake/_build/latest?definitionId=1) | |
| Azure Pipelines             | Debian        | [![Azure Pipelines Debian Build status](https://dev.azure.com/cake-build/Cake/_apis/build/status/Azure%20Pipelines%20-%20Build%20Cake%20Debian%20Stretch?&branchName=develop)](https://dev.azure.com/cake-build/Cake/_build/latest?definitionId=7) | |
| Azure Pipelines             | Fedora        | [![Azure Pipelines Fedora Build status](https://dev.azure.com/cake-build/Cake/_apis/build/status/Azure%20Pipelines%20-%20Build%20Cake%20Fedora%2028?&branchName=develop)](https://dev.azure.com/cake-build/Cake/_build/latest?definitionId=6) | |
| Azure Pipelines             | Centos        | [![Azure Pipelines Cake Centos status](https://dev.azure.com/cake-build/Cake/_apis/build/status/Azure%20Pipelines%20-%20Build%20Cake%20Centos%207?&branchName=develop)](https://dev.azure.com/cake-build/Cake/_build/latest?definitionId=5) | |
| Azure Pipelines             | Ubuntu        | [![Azure Pipelines Ubuntu Build status](https://dev.azure.com/cake-build/Cake/_apis/build/status/Azure%20Pipelines%20-%20Build%20Cake%20Ubuntu?&branchName=develop)](https://dev.azure.com/cake-build/Cake/_build/latest?definitionId=3) | |
| AppVeyor                    | Windows       | [![AppVeyor branch](https://img.shields.io/appveyor/ci/cakebuild/cake/develop.svg)](https://ci.appveyor.com/project/cakebuild/cake/branch/develop)                  | [![AppVeyor branch](https://img.shields.io/appveyor/ci/cakebuild/cake-eijwj/develop.svg)](https://ci.appveyor.com/project/cakebuild/cake-eijwj)  |
| TeamCity                    | Windows       | [![TeamCity Build Status](http://img.shields.io/teamcity/codebetter/Cake_CakeMaster.svg)](http://teamcity.codebetter.com/viewType.html?buildTypeId=Cake_CakeMaster) |                                                                                                                                                                     |
| Bitrise                     | MacOS         | [![Build Status](https://app.bitrise.io/app/42eaef77e8db4a5c/status.svg?token=EDjHGK5njNJ-MrhSbvKM1w&branch=develop)](https://app.bitrise.io/app/42eaef77e8db4a5c)  | ![Build Status](https://app.bitrise.io/app/804b431c1f27e0a0/status.svg?token=qKosHEaJAJEqzZcq4s5WRg&branch=develop)                                                        |
| Bitrise                     | Debian         | [![Build Status](https://app.bitrise.io/app/ea0c6b3c61eb1e79/status.svg?token=KJqOWXllYXz3WYqcB861Uw&branch=develop)](https://app.bitrise.io/app/ea0c6b3c61eb1e79)  | ![Build Status](https://app.bitrise.io/app/5a406f34f22113c6/status.svg?token=TQPbsmA9yP-iJOhzunIP4w&branch=develop)                                                        |
| Bitbucket Pipelines         | Debian         | [![Build Status](https://cakebitbucketpipelinesshield.azurewebsites.net/status/cakebuild/cake-integration-tests/develop)](https://cakebitbucketpipelinesshield.azurewebsites.net/url/cakebuild/cake-integration-tests/develop) |  |
| GitLab                      | Debian      | [![pipeline status](https://gitlab.com/cake-build/cake/badges/develop/pipeline.svg)](https://gitlab.com/cake-build/cake/commits/develop) |  &nbsp;                                                                                                                                                             |
| GitHub Actions              | Windows / Ubuntu/ macOS | [![Build Status](https://github.com/cake-build/cake/workflows/Build/badge.svg?branch=develop)](https://github.com/cake-build/cake/actions) | &nbsp; |

## Code Coverage

[![Coverage Status](https://coveralls.io/repos/github/cake-build/cake/badge.svg?branch=develop)](https://coveralls.io/github/cake-build/cake?branch=develop)

## Table of Contents

1. [Documentation](https://github.com/cake-build/cake#documentation)
2. [Contributing](https://github.com/cake-build/cake#contributing)
3. [Get in touch](https://github.com/cake-build/cake#get-in-touch)
4. [License](https://github.com/cake-build/cake#license)

## Documentation

You can read the latest documentation at [https://cakebuild.net/](https://cakebuild.net/).

For a simple example to get started see [Setting up a new project](https://cakebuild.net/docs/getting-started/setting-up-a-new-project).

## Contributing

So you’re thinking about contributing to Cake? Great! It’s **really** appreciated.

Make sure you've read the [contribution guidelines](https://cakebuild.net/docs/contributing/contribution-guidelines) before sending that epic pull request. You'll also need to sign the [contribution license agreement](https://cla.dotnetfoundation.org/cake-build/cake) (CLA) for anything other than a trivial change.  **NOTE:** The .NET Foundation CLA Bot will provide a link to this CLA within the PR that you submit if it is deemed as required.

* Fork the repository.
* Create a branch to work in.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request.

## Get in touch

[![Follow @cakebuildnet](https://img.shields.io/badge/Twitter-Follow%20%40cakebuildnet-blue.svg)](https://twitter.com/intent/follow?screen_name=cakebuildnet)

[![Join the chat at https://github.com/cake-build/cake/discussions](https://img.shields.io/badge/discussions-join%20chat-brightgreen)](https://github.com/cake-build/cake/discussions?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## License

Copyright © .NET Foundation, Patrik Svensson, Mattias Karlsson, Gary Ewan Park, Alistair Chapman, Martin Björkström, Dave Glick, Pascal Berger, Jérémie Desautels, Enrico Campidoglio, C. Augusto Proiete, Nils Andresen, and contributors.

Cake is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/cake-build/cake/blob/develop/LICENSE).

* For Roslyn, see https://github.com/dotnet/roslyn/blob/master/License.txt
* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE
* For NuGet.Core, see https://github.com/NuGet/Home/blob/dev/LICENSE.txt

## Thanks

A big thank you has to go to [JetBrains](https://www.jetbrains.com) who provide each of the Cake Developers with an [Open Source License](https://www.jetbrains.com/community/opensource/#support) for [ReSharper](https://www.jetbrains.com/resharper/) that helps with the development of Cake.

### Sponsors

Our wonderful sponsors:

[![Sponsors](https://opencollective.com/cake/sponsors.svg)](https://opencollective.com/cake)

### Backers

Our wonderful backers:

[![Backers](https://opencollective.com/cake/backers.svg)](https://opencollective.com/cake)

## Code of Conduct

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

## Contribution License Agreement

By signing the [CLA](https://cla.dotnetfoundation.org/cake-build/cake), the community is free to use your contribution to .NET Foundation projects.

## .NET Foundation

This project is supported by the [.NET Foundation](http://www.dotnetfoundation.org).
