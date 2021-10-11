using System;

namespace Discoverer.Logic.Grid.Isometric
{
    public class IsometricBuilder : IGridBuilder
    {
        private readonly int _width;
        private readonly int _height;
        
        public IsometricBuilder(int width, int height)
        {
            _width = width;
            _height = height;
        }
        
        public IGrid<T> BuildGrid<T>()
        {
            return new IsometricGrid<T>(_width, _height);
        }
        
        public ICoordinateRandom BuildRandom(Random random)
        {
            return new IsometricCoordinateRandom(_width, _height, random);
        }
        
        public ICoordinateHelper BuildCoordinateHelper()
        {
            return new IsometricCoordinateHelper();
        }
    }
}