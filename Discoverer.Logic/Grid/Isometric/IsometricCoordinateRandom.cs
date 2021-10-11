using System;

namespace Discoverer.Logic.Grid.Isometric
{
    public class IsometricCoordinateRandom : ICoordinateRandom
    {
        private readonly Random _random;
        
        private readonly int _width;
        private readonly int _height;
        
        public IsometricCoordinateRandom(int width, int height, Random random)
        {
            _width = width;
            _height = height;
            _random = random;
        }
        
        public ICoordinate Next()
        {
            return new IsometricCoordinate
            {
                X = _random.Next(0, _width),
                Y = _random.Next(0, _height)
            };
        }
    }
}