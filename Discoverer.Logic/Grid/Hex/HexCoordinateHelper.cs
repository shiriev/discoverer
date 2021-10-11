using System;

namespace Discoverer.Logic.Grid.Hex
{
    public class HexCoordinateHelper : ICoordinateHelper
    {
        public int CalculateDistance(ICoordinate a, ICoordinate b)
        {
            if (a is not HexCoordinate hexA) throw new ArgumentException(nameof(a));
            if (b is not HexCoordinate hexB) throw new ArgumentException(nameof(b));
            
            var dx = Math.Abs(hexA.X - hexB.X);
            var dy = Math.Abs(hexA.Y - hexB.Y);
            if (dx % 2 == 1)
            {
                if (hexA.X % 2 == 1)
                {
                    dy += hexA.Y > hexB.Y ? 1 : 0;
                }
                if (hexB.X % 2 == 1)
                {
                    dy += hexB.Y > hexA.Y ? 1 : 0;
                }
            }
            
            return Math.Max(dx, dy + (dx) / 2);
        }
        
        public bool SamePoint(ICoordinate a, ICoordinate b)
        {
            if (a is not HexCoordinate hexA) throw new ArgumentException(nameof(a));
            if (b is not HexCoordinate hexB) throw new ArgumentException(nameof(b));

            return hexA.X == hexB.X && hexA.Y == hexB.Y;
        }
    }
}