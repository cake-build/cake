#Cake [![NuGet](https://img.shields.io/nuget/v/Cake.svg)](https://www.nuget.org/packages/Cake) [![MyGet](https://img.shields.io/myget/cake/v/Cake.svg)](https://www.myget.org/gallery/cake)

Cake (C# Make) is a build automation system with a C# DSL to do things like compiling code, copy files/folders, running unit tests, compress files and build NuGet packages.

| Platform    | Status                                                                                                                    |
|-------------|---------------------------------------------------------------------------------------------------------------------------|
| Windows     | [![AppVeyor branch](https://img.shields.io/appveyor/ci/cakebuild/cake/develop.svg)](https://ci.appveyor.com/project/cakebuild/cake/branch/develop)      |
| Linux / OS X | [![Travis build status](https://travis-ci.org/cake-build/cake.svg?branch=develop)](https://travis-ci.org/cake-build/cake) |

## Table of contents

1. [Documentation](#documentation)
2. [Example](#example)
    - [Install the Cake bootstrapper](#1-install-the-cake-bootstrapper)
    - [Create a Cake script](#2-create-a-cake-script)
    - [Run it!](#3-run-it)
3. [Contributing](#contributing)
4. [Get in touch](#get-in-touch)
5. [License](#license)

## Documentation

You can read the latest documentation at [http://cakebuild.net/](http://cakebuild.net/).

## Example

This example dowloads the Cake bootstrapper and executes a simple build script.
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
Invoke-WebRequest http://cakebuild.net/bootstrapper/windows -OutFile build.ps1
```

##### Linux

```console
curl -Lsfo build.sh http://cakebuild.net/bootstrapper/linux
```

##### OS X

```console
curl -Lsfo build.sh http://cakebuild.net/bootstrapper/osx
```

### 2. Create a Cake script

Add a cake script called `build.cake` to the same location as the
bootstrapper script that you downloaded.

```csharp
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
bash build.sh
```

## Contributing

So you’re thinking about contributing to Cake? Great! It’s **really** appreciated.   

Make sure you've read the [contribution guidelines](http://cakebuild.net/contribute/contribution-guidelines/) before sending that epic pull request.

* Fork the repository.
* Make your feature addition or bug fix.
* Don't forget the unit tests.
* Send a pull request.

## Get in touch

[![Follow @cakebuildnet](https://img.shields.io/badge/Twitter-Follow%20%40cakebuildnet-blue.svg)](https://twitter.com/intent/follow?screen_name=cakebuildnet)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## License

Copyright © 2014 - 2015, Patrik Svensson, Mattias Karlsson, Gary Ewan Park and contributors.
Cake is provided as-is under the MIT license. For more information see [LICENSE](LICENSE).

* For Roslyn, see https://github.com/dotnet/roslyn/blob/master/License.txt
* For Mono.CSharp, see https://github.com/mono/mono/blob/master/mcs/LICENSE
* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE
* For NuGet.Core, see https://nuget.codeplex.com/license
