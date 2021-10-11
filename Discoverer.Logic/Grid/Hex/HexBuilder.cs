using System;

namespace Discoverer.Logic.Grid.Hex
{
    public class HexBuilder : IGridBuilder
    {
        private readonly int _width;
        private readonly int _height;
        
        public HexBuilder(int width, int height)
        {
            _width = width;
            _height = height;
        }
        
        public IGrid<T> BuildGrid<T>()
        {
            return new HexGrid<T>(_width, _height);
        }
        
        public ICoordinateRandom BuildRandom(Random random)
        {
            return new HexCoordinateRandom(_width, _height, random);
        }
        
        public ICoordinateHelper BuildCoordinateHelper()
        {
            return new HexCoordinateHelper();
        }
    }
}