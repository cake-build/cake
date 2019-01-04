#!/usr/bin/env bash

TARGET="Run-All-Tests"
SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
ROOT_DIR=$SCRIPT_DIR/../..

# Parse arguments.
for i in "$@"; do
    case $1 in
        -t|--target) TARGET="$2"; shift ;;
        -s|--skipbuildingcake) SKIP_BUILDING_CAKE="--exclusive" ;;
    esac
    shift
done


#####################################################################
# RUN TESTS
#####################################################################

pushd $ROOT_DIR >/dev/null
./build.sh --target="Run-Integration-Tests" --integration-tests-target="$TARGET" $SKIP_BUILDING_CAKE
INTEGRATION_TEST_EXIT_CODE=$?
popd >/dev/null
exit "$INTEGRATION_TEST_EXIT_CODE"