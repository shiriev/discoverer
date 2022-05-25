using System.Collections.Generic;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Api.Controllers.Contracts;

public class GameStateResponse
{
    public EGameState GameState { get; set; }
    
    public List<GameAction> AllActions { get; set; }
    
    public int CurrentPlayerNum { get; set; }
    
    public int PlayerCount { get; set; }
    
    public int CurrentTurn { get; set; }
    
    public List<List<Region>> RegionGrid { get; set; }
    
    public List<List<MarkerSet>> MarkerSetGrid { get; set; }
    
    public EGridType GridType { get; set; }
}