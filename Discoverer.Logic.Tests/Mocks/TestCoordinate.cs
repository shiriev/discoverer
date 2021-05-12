using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Tests.Mocks
{
    public class TestCoordinate : ICoordinate
    {
        public int I { get; init; }
            
        public override bool Equals(object? obj)
        {
            if (obj is not TestCoordinate newCoord)
                return false;
            return I == newCoord.I;
        }

        public override int GetHashCode()
        {
            return I;
        }

        public override string ToString()
        {
            return $"TEST({I})";
        }
    }
}