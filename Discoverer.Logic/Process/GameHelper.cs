using System;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;

namespace Discoverer.Logic.Process
{
    public partial class Game
    {
        private HexHelper? _hex;
        private IsometricHelper? _isometric;
        
        public HexHelper Hex => _hex ??= new HexHelper(this);
        
        public IsometricHelper Isometric => _isometric ??= new IsometricHelper(this);
    }
    
    public class HexHelper
    {
        private readonly Game _game;
        public HexHelper(Game game)
        {
            _game = game;
        }
        
        public HexCoordinate Grail => _game.GetGrail() as HexCoordinate ?? throw new ArgumentException();
        
        public HexGrid<Region> RegionGrid => _game.GetRegionGrid() as HexGrid<Region> ?? throw new ArgumentException();
        
        public HexGrid<MarkerSet> MarkerSetGrid => _game.GetMarkerSetGrid() as HexGrid<MarkerSet> ?? throw new ArgumentException();
    }
    
    public class IsometricHelper
    {
        private readonly Game _gameProcess;
        public IsometricHelper(Game gameProcess)
        {
            _gameProcess = gameProcess;
        }
        
        public IsometricCoordinate Grail => _gameProcess.GetGrail() as IsometricCoordinate ?? throw new ArgumentException();
        
        public IsometricGrid<Region> RegionGrid => _gameProcess.GetRegionGrid() as IsometricGrid<Region> ?? throw new ArgumentException();
        
        public IsometricGrid<MarkerSet> MarkerSetGrid => _gameProcess.GetMarkerSetGrid() as IsometricGrid<MarkerSet> ?? throw new ArgumentException();
    }
}