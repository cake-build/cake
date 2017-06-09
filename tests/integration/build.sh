#!/usr/bin/env bash

#####################################################################
# FUNCTIONS
#####################################################################

function cleanDirectory()
{
    if [ ! -z "$1" ]; then
        if [ -d $1 ]; then
            rm -rf -- $1
        fi
        mkdir $1
    fi
}

#####################################################################
# PREPARATION
#####################################################################

TARGET="Run-All-Tests"
SKIP_BUILDING_CAKE=0
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
SCRIPT=$SCRIPT_DIR/build.cake
TOOLS_DIR=$SCRIPT_DIR/tools
NUGET_EXE=$TOOLS_DIR/nuget.exe
NUGET_URL="https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

# Parse arguments.
for i in "$@"; do
    case $1 in
        -t|--target) TARGET="$2"; shift ;;
        -s|--skipbuildingcake) SKIP_BUILDING_CAKE=1 ;;
    esac
    shift
done

mono --version

# Make sure the tools folder exist.
if [ ! -d $TOOLS_DIR ]; then
  mkdir $TOOLS_DIR
fi

# Download NuGet if it does not exist.
if [ ! -f $NUGET_EXE ]; then
    echo "Downloading NuGet..."
    curl -Lsfo $NUGET_EXE $NUGET_URL
    if [ $? -ne 0 ]; then
        echo "An error occured while downloading nuget.exe."
        exit 1
    fi
fi

###########################################################################
# INSTALL .NET CORE CLI
###########################################################################

echo "Installing .NET CLI..."
if [ ! -d "$SCRIPT_DIR/.dotnet" ]; then
  mkdir "$SCRIPT_DIR/.dotnet"
fi
curl -Lsfo "$SCRIPT_DIR/.dotnet/dotnet-install.sh" https://dot.net/v1/dotnet-install.sh
sudo bash "$SCRIPT_DIR/.dotnet/dotnet-install.sh" --version 1.0.4 --install-dir .dotnet --no-path
export PATH="$SCRIPT_DIR/.dotnet":$PATH
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
"$SCRIPT_DIR/.dotnet/dotnet" --info


#####################################################################
# BUILD CAKE
#####################################################################

ROOT_DIR=$SCRIPT_DIR/../..
ARTIFACTS_DIR=$ROOT_DIR/artifacts

# Build Cake.
if [ ${SKIP_BUILDING_CAKE} -eq 0 ]; then
    $(cleanDirectory $ARTIFACTS_DIR)
    pushd $ROOT_DIR >/dev/null
    echo "Building Cake..."
    ./build.sh --target Copy-Files >/dev/null
    popd >/dev/null
fi

# Get the built Cake path.
BUILT_CAKE_DIR=$(dirname $(find $ARTIFACTS_DIR -name 'Cake.exe'))
if [ ! -d $BUILT_CAKE_DIR ]; then
    echo "Could not locate built Cake."
    exit 1
fi

# Get the built Cake CorCLR path.
BUILT_CAKE_CORECLR_DIR=$(dirname $(find $ARTIFACTS_DIR -name 'Cake.dll'))
if [ ! -d $BUILT_CAKE_CORECLR_DIR ]; then
    echo "Could not locate built Cake CoreCLR."
    exit 1
fi

# Get the local Cake path.
CAKE_DIR=$TOOLS_DIR/Cake
CAKE_EXE=$CAKE_DIR/Cake.exe
CAKE_CORECLR_DIR=$TOOLS_DIR/Cake.CoreCLR
CAKE_DLL=$CAKE_CORECLR_DIR/Cake.dll

# Clean the local Cake path.
if [ ${SKIP_BUILDING_CAKE} -eq 0 ]; then
    $(cleanDirectory $CAKE_DIR) >/dev/null
    $(cleanDirectory $CAKE_CORECLR_DIR) >/dev/null
fi

# Copy the built Cake to the local Cake path.
if [ ${SKIP_BUILDING_CAKE} -eq 0 ]; then
    cp -r "$BUILT_CAKE_DIR/"* "$CAKE_DIR/"
    cp -r "$BUILT_CAKE_CORECLR_DIR/"* "$CAKE_CORECLR_DIR/"
fi

# Ensure that Cake can be found where we expect it to.
if [ ! -f $CAKE_EXE ]; then
    echo "Could not find Cake.exe at '$CAKE_EXE'."
    exit 1
fi

# Ensure that Cake CoreLCR can be found where we expect it to.
if [ ! -f $CAKE_DLL ]; then
    echo "Could not find CoreCLR Cake.dll at '$CAKE_DLL'."
    exit 1
fi

#####################################################################
# SETUP ENVIRONMENT
#####################################################################

export MyEnvironmentVariable="Hello World"

#####################################################################
# RUN TESTS
#####################################################################

mono $CAKE_EXE --version
echo "Running Mono integration tests..."
mono $CAKE_EXE "$SCRIPT" "--target=$TARGET" "--verbosity=quiet" "--platform=posix" "--customarg=hello"

dotnet $CAKE_DLL --version
echo "Running CoreCLR integration tests..."
dotnet $CAKE_DLL "$SCRIPT" "--target=$TARGET" "--verbosity=quiet" "--platform=posix" "--customarg=hello"