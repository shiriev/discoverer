using System;
using System.Collections.Immutable;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Contracts
{
    public class GameSave
    {
        public ImmutableGrid<Region> Grid { get; set; }
        
        public EHint[] Hints { get; set; }
        
        public ICoordinate Grail { get; set; }
        
        public ImmutableList<GameAction> Actions { get; set; }
        
        public IImmutableGrid<MarkerSet> MarkerSetGrid { get; set; }
        
        public int CurrentPlayerNum { get; set; }
        
        public int CurrentTurn { get; set; }
        
        public GameState GameState { get; set; }
        
        public int PlayerCount { get; set; }
        
        public Guid GameId { get; set; }
        
        public GameSettings GameSettings { get; set; }
    }
}