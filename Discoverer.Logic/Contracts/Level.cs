using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;

namespace Discoverer.Logic.Contracts
{
    internal class Level
    {
        public ImmutableGrid<Region> Grid { get; set; }
        
        public EHint[] Hints { get; set; }
        
        public ICoordinate Grail { get; set; }
    }
}