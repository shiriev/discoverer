using System;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;

namespace Discoverer.Logic.Process
{
    public partial class GameProcess
    {
        private HexHelper? _hex;
        private IsometricHelper? _isometric;
        
        public HexHelper Hex => _hex ??= new HexHelper(this);
        
        public IsometricHelper Isometric => _isometric ??= new IsometricHelper(this);
    }
    
    public class HexHelper
    {
        private readonly GameProcess _gameProcess;
        public HexHelper(GameProcess gameProcess)
        {
            _gameProcess = gameProcess;
        }
        
        public HexCoordinate Grail => _gameProcess.GetGrail() as HexCoordinate ?? throw new ArgumentException();
        
        public HexGrid<Region> RegionGrid => _gameProcess.GetRegionGrid() as HexGrid<Region> ?? throw new ArgumentException();
        
        public HexGrid<MarkerSet> MarkerSetGrid => _gameProcess.GetMarkerSetGrid() as HexGrid<MarkerSet> ?? throw new ArgumentException();
    }
    
    public class IsometricHelper
    {
        private readonly GameProcess _gameProcess;
        public IsometricHelper(GameProcess gameProcess)
        {
            _gameProcess = gameProcess;
        }
        
        public IsometricCoordinate Grail => _gameProcess.GetGrail() as IsometricCoordinate ?? throw new ArgumentException();
        
        public IsometricGrid<Region> RegionGrid => _gameProcess.GetRegionGrid() as IsometricGrid<Region> ?? throw new ArgumentException();
        
        public IsometricGrid<MarkerSet> MarkerSetGrid => _gameProcess.GetMarkerSetGrid() as IsometricGrid<MarkerSet> ?? throw new ArgumentException();
    }
}