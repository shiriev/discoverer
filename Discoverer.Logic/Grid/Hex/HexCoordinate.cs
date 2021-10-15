namespace Discoverer.Logic.Grid.Hex
{
    public class HexCoordinate : ICoordinate
    {
        public int X { get; init; }
        public int Y { get; init; }

        public override string ToString()
        {
            return $"HEX({X}, {Y})";
        }
    }
}