using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process.Contracts
{
    internal class UpdateGameCastRequest
    {
        public ImmutableList<GameAction>? Actions { get; set; }
        
        public IImmutableGrid<MarkerSet>? MarkerSetGrid { get; set; }
        
        public int? CurrentPlayerNum { get; set; }
        
        public int? CurrentTurn { get; set; }
        
        public GameState? GameState { get; set; }
    }
}