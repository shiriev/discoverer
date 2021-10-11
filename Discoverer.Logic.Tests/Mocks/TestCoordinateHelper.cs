using System;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Tests.Mocks
{
    public class TestCoordinateHelper : ICoordinateHelper
    {
        public int CalculateDistance(ICoordinate a, ICoordinate b)
        {
            if (a is not TestCoordinate testA) throw new ArgumentException(nameof(a));
            if (b is not TestCoordinate testB) throw new ArgumentException(nameof(b));
            
            return (Math.Abs(testA.I - testB.I));
        }

        public bool SamePoint(ICoordinate a, ICoordinate b)
        {
            if (a is not TestCoordinate testA) throw new ArgumentException(nameof(a));
            if (b is not TestCoordinate testB) throw new ArgumentException(nameof(b));

            return testA.I == testB.I;
        }
    }
}