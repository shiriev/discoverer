using System;

namespace Discoverer.Logic.Grid.Isometric
{
    public class IsometricCoordinateHelper : ICoordinateHelper
    {
        public int CalculateDistance(ICoordinate a, ICoordinate b)
        {
            if (a is not IsometricCoordinate isoA) throw new ArgumentException(nameof(a));
            if (b is not IsometricCoordinate isoB) throw new ArgumentException(nameof(b));
            
            return Math.Max(Math.Abs(isoA.X - isoB.X), Math.Abs(isoA.Y - isoB.Y));
        }

        public bool SamePoint(ICoordinate a, ICoordinate b)
        {
            if (a is not IsometricCoordinate isoA) throw new ArgumentException(nameof(a));
            if (b is not IsometricCoordinate isoB) throw new ArgumentException(nameof(b));

            return isoA.X == isoB.X && isoA.Y == isoB.Y;
        }
    }
}