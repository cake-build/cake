// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Graph
{
    internal sealed class CakeGraphEdge
    {
        private readonly string _start;
        private readonly string _end;

        public string Start
        {
            get { return _start; }
        }

        public string End
        {
            get { return _end; }
        }

        public CakeGraphEdge(string start, string end)
        {
            _start = start;
            _end = end;
        }
    }
}
