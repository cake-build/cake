#!/usr/bin/env bash
# Define varibles
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
TOOLS_DIR=$SCRIPT_DIR/tools


# Make sure the tools folder exist.
if [ ! -d "$TOOLS_DIR" ]; then
  mkdir "$TOOLS_DIR"
fi

###########################################################################
# INSTALL .NET CORE CLI
###########################################################################

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
export DOTNET_ROLL_FORWARD_ON_NO_CANDIDATE_FX=2

if [ ! -d "$SCRIPT_DIR/.dotnet" ]; then
    mkdir "$SCRIPT_DIR/.dotnet"
fi
curl -Lsfo "$SCRIPT_DIR/.dotnet/dotnet-install.sh" https://dot.net/v1/dotnet-install.sh

# Install additional SDK channels if CAKE_INSTALL_SUPPORTED_SDKS is set to true
if [ "$CAKE_INSTALL_SUPPORTED_SDKS" = "true" ]; then
    echo "Installing additional supported SDK channels..."
    
    # Install .NET 8.0 SDK
    echo "Installing .NET 8.0 SDK..."
    bash "$SCRIPT_DIR/.dotnet/dotnet-install.sh" --channel 8.0 --install-dir .dotnet --no-path
    
    # Install .NET 9.0 SDK
    echo "Installing .NET 9.0 SDK..."
    bash "$SCRIPT_DIR/.dotnet/dotnet-install.sh" --channel 9.0 --install-dir .dotnet --no-path --skip-non-versioned-files
    
    # Install .NET 10.0 SDK (preview quality)
    echo "Installing .NET 10.0 SDK (preview)..."
    bash "$SCRIPT_DIR/.dotnet/dotnet-install.sh" --channel 10.0 --quality preview --install-dir .dotnet --no-path --skip-non-versioned-files
fi

# Install SDK from global.json
bash "$SCRIPT_DIR/.dotnet/dotnet-install.sh" --jsonfile ./global.json --install-dir .dotnet --no-path

# Install specific SDK version if CAKE_INSTALL_SDK_VERSION is set
if [ "$CAKE_INSTALL_SDK_VERSION" ]; then
    echo "Installing .NET $CAKE_INSTALL_SDK_VERSION SDK..."
    bash "$SCRIPT_DIR/.dotnet/dotnet-install.sh" --version $CAKE_INSTALL_SDK_VERSION --install-dir .dotnet --no-path
fi

export PATH="$SCRIPT_DIR/.dotnet":$PATH
export DOTNET_ROOT="$SCRIPT_DIR/.dotnet"

dotnet --info

###########################################################################
# INSTALL CAKE
###########################################################################

dotnet tool restore

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

dotnet cake "$@"