using System.Collections.Generic;

namespace Discoverer.Logic.GameContract
{
    public class CellState
    {
        public Player ImproperCellForPlayer { get; init; }
        
        public IReadOnlySet<Player> PossibleCellForPlayers { get; init; }
    }
}