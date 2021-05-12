using System;

namespace Discoverer.Logic.Grid.Hex
{
    public class HexCoordinateHelper : ICoordinateHelper<HexCoordinate> 
    {
        public int CalculateDistance(HexCoordinate a, HexCoordinate b)
        {
            var dx = Math.Abs(a.X - b.X);
            var dy = Math.Abs(a.Y - b.Y);
            if (dx % 2 == 1)
            {
                if (a.X % 2 == 1)
                {
                    dy += a.Y > b.Y ? 1 : 0;
                }
                if (b.X % 2 == 1)
                {
                    dy += b.Y > a.Y ? 1 : 0;
                }
            }
            
            return Math.Max(dx, dy + (dx) / 2);
        }
        
    }
}