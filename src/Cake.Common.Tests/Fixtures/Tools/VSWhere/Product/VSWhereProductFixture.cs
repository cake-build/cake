// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.VSWhere.Product;

namespace Cake.Common.Tests.Fixtures.Tools.VSWhere.Product
{
    internal sealed class VSWhereProductFixture : VSWhereFixture<VSWhereProductSettings>
    {
        public VSWhereProductFixture()
        {
            Settings.Products = "Microsoft.VisualStudio.Product.BuildTools";
        }

        protected override void RunTool()
        {
            var tool = new VSWhereProduct(FileSystem, Environment, ProcessRunner, Tools);
            tool.Products(Settings);
        }
    }
}
