using System;

namespace Discoverer.Logic.Grid.Isometric
{
    public class IsometricCoordinateHelper : ICoordinateHelper<IsometricCoordinate> 
    {
        public int CalculateDistance(IsometricCoordinate a, IsometricCoordinate b)
        {
            return Math.Max(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
        }
    }
}