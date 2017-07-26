# Cake

[![NuGet](https://img.shields.io/nuget/v/Cake.svg)](https://www.nuget.org/packages/Cake) [![MyGet](https://img.shields.io/myget/cake/vpre/Cake.svg?label=myget)](https://www.myget.org/gallery/cake) [![Chocolatey](https://img.shields.io/chocolatey/v/Cake.portable.svg)](https://chocolatey.org/packages/cake.portable)
[![homebrew](https://img.shields.io/homebrew/v/cake.svg)](http://braumeister.org/formula/cake)

[![Source Browser](https://img.shields.io/badge/Browse-Source-green.svg)](http://sourcebrowser.io/Browse/cake-build/cake)

Cake (C# Make) is a build automation system with a C# DSL to do things like compiling code, copy files/folders, running unit tests, compress files and build NuGet packages.

## Continuous integration

| Build server                | Platform     | Build status                                                                                                                                                        | Integration tests                                                                                                                                                   |
|-----------------------------|--------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| AppVeyor                    | Windows      | [![AppVeyor branch](https://img.shields.io/appveyor/ci/cakebuild/cake/develop.svg)](https://ci.appveyor.com/project/cakebuild/cake/branch/develop)                  | [![AppVeyor branch](https://img.shields.io/appveyor/ci/cakebuild/cake-eijwj/develop.svg)](https://ci.appveyor.com/project/cakebuild/cake-eijwj)  |
| Travis                      | Linux / OS X | [![Travis build status](https://travis-ci.org/cake-build/cake.svg?branch=develop)](https://travis-ci.org/cake-build/cake)                                           |                                                                                                                                                                     |
| TeamCity                    | Windows      | [![TeamCity Build Status](http://img.shields.io/teamcity/codebetter/Cake_CakeMaster.svg)](http://teamcity.codebetter.com/viewType.html?buildTypeId=Cake_CakeMaster) |                                                                                                                                                                     |
| Bitrise                     | OS X         | ![Bitrise Build Status](https://www.bitrise.io/app/7a9d707b00881436.svg?token=m8zsF3tNONLaF03eHU-Ftg&branch=develop)                                                | ![Build Status](https://www.bitrise.io/app/804b431c1f27e0a0.svg?token=qKosHEaJAJEqzZcq4s5WRg&branch=develop)                                                        |
| Bitrise                     | Linux        | ![Bitrise Build Status](https://www.bitrise.io/app/b811c91a26b1ea80.svg?token=zdwab0niOTRF4p3HcFYaxQ&branch=develop)                                                | ![Build Status](https://www.bitrise.io/app/5a406f34f22113c6.svg?token=TQPbsmA9yP-iJOhzunIP4w&branch=develop)                                                        |
| Jenkins                     | Windows      | [![Jenkins](https://img.shields.io/jenkins/s/https/cakejenkins.azurewebsites.net/Cake.svg)](https://cakejenkins.azurewebsites.net/job/Cake/lastStableBuild/)       |                                                                                                                                                                     |
| Bamboo                      | Windows      | [![Bamboo Build Status](https://cakebambooshield.azurewebsites.net/planstatus/Flat/CAKE-CAKE.svg)](https://bamboo.devlead.se/browse/CAKE-CAKE/latest)             | [![Bamboo Build Status](https://cakebambooshield.azurewebsites.net/planstatus/Flat/CAKE-IT.svg)](https://bamboo.devlead.se/browse/CAKE-IT/latest)   |
| Visual Studio Team Services | Windows      | ![VSTS Build Status](https://img.shields.io/vso/build/cake-build/af63183c-ac1f-4dbb-93bc-4fa862ea5809/1.svg)                                                        |                                                                                                                                                                     |
| MyGet Build Services        | Windows      | [![MyGet Build Status](https://www.myget.org/BuildSource/Badge/cake-myget-build-service?identifier=53513546-050e-45de-9500-f161c99df6e2)](https://www.myget.org/)   |  &nbsp;                                                                                                                                                             |
| Bitbucket Pipelines         | Linux        | [![Build Status](https://cakebitbucketpipelinesshield.azurewebsites.net/status/cakebuild/cake-integration-tests/develop)](https://cakebitbucketpipelinesshield.azurewebsites.net/url/cakebuild/cake-integration-tests/develop) | [![Build Status](https://cakebitbucketpipelinesshield.azurewebsites.net/status/cakebuild/cake-integration-tests/IntegrationTests_develop)](https://cakebitbucketpipelinesshield.azurewebsites.net/url/cakebuild/cake-integration-tests/IntegrationTests_develop) |
| GitLabs                     | Linux      | [![build status](https://gitlab.com/cake-build/cake/badges/develop/build.svg)](https://gitlab.com/cake-build/cake/builds) |  &nbsp;                                                                                                                                                             |

## Code Coverage

[![Coverage Status](https://coveralls.io/repos/github/cake-build/cake/badge.svg?branch=develop)](https://coveralls.io/github/cake-build/cake?branch=develop)

## Table of Contents

1. [Documentation](https://github.com/cake-build/cake#documentation)
2. [Example](https://github.com/cake-build/cake#example)
    - [Install the Cake bootstrapper](https://github.com/cake-build/cake#1-install-the-cake-bootstrapper)
    - [Create a Cake script](https://github.com/cake-build/cake#2-create-a-cake-script)
    - [Run it!](https://github.com/cake-build/cake#3-run-it)
3. [Contributing](https://github.com/cake-build/cake#contributing)
4. [Get in touch](https://github.com/cake-build/cake#get-in-touch)
5. [License](https://github.com/cake-build/cake#license)

## Documentation

You can read the latest documentation at [http://cakebuild.net/](http://cakebuild.net/).

## Example

This example downloads the Cake bootstrapper and executes a simple build script.
The bootstrapper is used to bootstrap Cake in a simple way and is not in
required in any way to execute build scripts. If you prefer to invoke the Cake
executable yourself, [take a look at the command line usage](http://cakebuild.net/docs/cli/usage).

This example is also available on our homepage:
[http://cakebuild.net/docs/tutorials/setting-up-a-new-project](http://cakebuild.net/docs/tutorials/setting-up-a-new-project)

### 1. Install the Cake bootstrapper

The bootstrapper is used to download Cake and the tools required by the
build script.

##### Windows

```powershell
Invoke-WebRequest http://cakebuild.net/download/bootstrapper/windows -OutFile build.ps1
```

##### Linux

```console
curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/linux
```

##### OS X

```console
curl -Lsfo build.sh http://cakebuild.net/download/bootstrapper/osx
```

### 2. Create a Cake script

Add a cake script called `build.cake` to the same location as the
bootstrapper script that you downloaded.

```cake
var target = Argument("target", "Default");

Task("Default")
  .Does(() =>
{
  Information("Hello World!");
});

RunTarget(target);
```

### 3. Run it!

##### Windows

```powershell
# Execute the bootstrapper script.
./build.ps1
```

##### Linux / OS X

```console
# Adjust the permissions for the bootstrapper script.
chmod +x build.sh

# Execute the bootstrapper script.
./build.sh
```

## Contributing

So you’re thinking about contributing to Cake? Great! It’s **really** appreciated.

Make sure you've read the [contribution guidelines](http://cakebuild.net/docs/contributing/contribution-guidelines) before sending that epic pull request. You'll also need to sign the [contribution license agreement](https://cla2.dotnetfoundation.org/) (CLA) for anything other than a trivial change.  **NOTE:** The .NET Foundation CLA Bot will provide a link to this CLA within the PR that you submit if it is deemed as required.

* Fork the repository.
* Create a branch to work in.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request.

## Get in touch

[![Follow @cakebuildnet](https://img.shields.io/badge/Twitter-Follow%20%40cakebuildnet-blue.svg)](https://twitter.com/intent/follow?screen_name=cakebuildnet)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## License

Copyright © .NET Foundation, Patrik Svensson, Mattias Karlsson, Gary Ewan Park, Alistair Chapman, Martin Björkström and contributors.

Cake is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/cake-build/cake/blob/develop/LICENSE).

* For Roslyn, see https://github.com/dotnet/roslyn/blob/master/License.txt
* For Mono.CSharp, see https://github.com/mono/mono/blob/master/mcs/LICENSE
* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE
* For NuGet.Core, see https://github.com/NuGet/Home/blob/dev/LICENSE.txt

## Thanks

A big thank you has to go to [JetBrains](https://www.jetbrains.com) who provide each of the Cake Developers with an [Open Source License](https://www.jetbrains.com/support/community/#section=open-source) for [ReSharper](https://www.jetbrains.com/resharper/) that helps with the development of Cake.

The Cake Team would also like to say thank you to the guys at [MyGet](https://www.myget.org/) for their support in providing a Professional Subscription which allows us to continue to push all of our pre-release editions of Cake NuGet packages for early consumption by the Cake Community.

## Code of Conduct

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

## Contribution License Agreement

By signing the [CLA](https://cla2.dotnetfoundation.org/), the community is free to use your contribution to .NET Foundation projects.

## .NET Foundation

This project is supported by the [.NET Foundation](http://www.dotnetfoundation.org).
