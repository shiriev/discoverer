using System.Collections.Immutable;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract
{
    public class ProcessUpdate
    {
        public ImmutableList<GameAction>? Actions { get; set; }
        
        public IImmutableGrid<MarkerSet>? MarkerSetGrid { get; set; }
        
        public int? CurrentPlayerNum { get; set; }
        
        public int? CurrentTurn { get; set; }
        
        public GameState? GameState { get; set; }
    }
}