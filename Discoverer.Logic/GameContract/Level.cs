using Discoverer.Logic.CellContract;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;

namespace Discoverer.Logic.GameContract
{
    public class Level
    {
        public IGrid<Region> Grid { get; set; }
        
        public EHint[] Hints { get; set; }
        
        public ICoordinate Grail { get; set; }
    }
}