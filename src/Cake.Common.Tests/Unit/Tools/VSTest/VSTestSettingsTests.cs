// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.VSTest;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.VSTest
{
    public sealed class VSTestSettingsTests
    {
        [Fact]
        public void Logger_and_LoggerName_are_in_sync_when_None_is_specified()
        {
            // Given
            var settings = new VSTestSettings();

            // When
#pragma warning disable 0618 // Type or member is obsolete
#pragma warning disable 0612 // Type or member is obsolete
            settings.Logger = VSTestLogger.None;
#pragma warning restore 0612 // Type or member is obsolete
#pragma warning restore 0618 // Type or member is obsolete

            // Then
            Assert.Equal(string.Empty, settings.LoggerName);
        }

        [Fact]
        public void Logger_and_LoggerName_are_in_sync_when_Trx_is_specified()
        {
            // Given
            var settings = new VSTestSettings();

            // When
#pragma warning disable 0618 // Type or member is obsolete
#pragma warning disable 0612 // Type or member is obsolete
            settings.Logger = VSTestLogger.Trx;
#pragma warning restore 0612 // Type or member is obsolete
#pragma warning restore 0618 // Type or member is obsolete

            // Then
            Assert.Equal("trx", settings.LoggerName);
        }

        [Fact]
        public void Logger_and_LoggerName_are_in_sync_when_AppVeyor_is_specified()
        {
            // Given
            var settings = new VSTestSettings();

            // When
#pragma warning disable 0618 // Type or member is obsolete
#pragma warning disable 0612 // Type or member is obsolete
            settings.Logger = VSTestLogger.AppVeyor;
#pragma warning restore 0612 // Type or member is obsolete
#pragma warning restore 0618 // Type or member is obsolete

            // Then
            Assert.Equal("AppVeyor", settings.LoggerName);
        }

        [Fact]
        public void Exception_is_thrown_when_setting_Logger_to_Custom()
        {
            var settings = new VSTestSettings();
#pragma warning disable 0618 // Type or member is obsolete
#pragma warning disable 0612 // Type or member is obsolete
            Assert.Throws<ArgumentException>(() => settings.Logger = VSTestLogger.Custom);
#pragma warning restore 0612 // Type or member is obsolete
#pragma warning restore 0618 // Type or member is obsolete
        }
    }
}