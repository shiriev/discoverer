using System;

namespace Discoverer.Logic.Grid.Hex
{
    public class HexCoordinateRandom : ICoordinateRandom<HexCoordinate>
    {
        private readonly Random _random;
        
        private readonly int _width;
        private readonly int _height;
        
        public HexCoordinateRandom(int width, int height, Random random)
        {
            _width = width;
            _height = height;
            _random = random;
        }
        
        public HexCoordinate Next()
        {
            return new()
            {
                X = _random.Next(0, _width),
                Y = _random.Next(0, _height)
            };
        }
    }
}