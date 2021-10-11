using System;
using System.Collections.Generic;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract
{
    public class ProcessUpdate
    {
        public List<GameAction>? Actions { get; set; }
        
        public IGrid<CellState>? CellStateGrid { get; set; }
        
        public int? CurrentPlayerNum { get; set; }
        
        public int? CurrentTurn { get; set; }
        
        public GameState? GameState { get; set; }
    }
}