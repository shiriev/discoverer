namespace Discoverer.Logic.Grid.Hex
{
    public class HexCoordinate : ICoordinate
    {
        public int X { get; init; }
        public int Y { get; init; }
        
        public override bool Equals(object? obj)
        {
            if (obj is not HexCoordinate newCoord)
                return false;
            return X == newCoord.X && Y == newCoord.Y;
        }

        public override int GetHashCode()
        {
            var hash = 34;
            hash = (17 * hash) + X.GetHashCode();
            hash = (17 * hash) + Y.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return $"HEX({X}, {Y})";
        }
    }
}