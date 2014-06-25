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