namespace Cake.Core.Graph
{
    internal sealed class CakeGraphEdge
    {
        private readonly CakeTask _start;
        private readonly CakeTask _end;

        public CakeTask Start
        {
            get { return _start; }
        }

        public CakeTask End
        {
            get { return _end; }
        }

        public CakeGraphEdge(CakeTask start, CakeTask end)
        {
            _start = start;
            _end = end;
        }
    }
}