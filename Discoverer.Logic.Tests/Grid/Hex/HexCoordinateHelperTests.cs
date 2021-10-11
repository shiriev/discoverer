using Discoverer.Logic.Grid.Hex;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Grid.Hex
{
    public class HexCoordinateHelperTests
    {
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(0, 0, 0, 3, 3)]

        [TestCase(2, 2, 1, 1, 1)]
        [TestCase(2, 2, 2, 1, 1)]
        [TestCase(2, 2, 3, 1, 1)]
        [TestCase(2, 2, 1, 2, 1)]
        [TestCase(2, 2, 2, 3, 1)]
        [TestCase(2, 2, 3, 2, 1)]
        [TestCase(2, 2, 2, 2, 0)]
        
        [TestCase(1, 1, 0, 1, 1)]
        [TestCase(1, 1, 1, 0, 1)]
        [TestCase(1, 1, 2, 1, 1)]
        [TestCase(1, 1, 0, 2, 1)]
        [TestCase(1, 1, 1, 2, 1)]
        [TestCase(1, 1, 2, 2, 1)]
        [TestCase(1, 1, 1, 1, 0)]
        
        [TestCase(2, 2, 0, 1, 2)]
        [TestCase(2, 2, 0, 2, 2)]
        [TestCase(2, 2, 0, 3, 2)]
        [TestCase(2, 2, 1, 0, 2)]
        [TestCase(1, 0, 2, 2, 2)]
        [TestCase(2, 2, 2, 0, 2)]
        [TestCase(2, 2, 3, 0, 2)]
        [TestCase(2, 2, 4, 1, 2)]
        [TestCase(2, 2, 4, 2, 2)]
        [TestCase(2, 2, 4, 3, 2)]
        [TestCase(2, 2, 1, 3, 2)]
        [TestCase(2, 2, 2, 4, 2)]
        [TestCase(2, 2, 3, 3, 2)]
        
        [TestCase(3, 2, 1, 1, 2)]
        [TestCase(3, 2, 1, 2, 2)]
        [TestCase(3, 2, 1, 3, 2)]
        [TestCase(3, 2, 2, 1, 2)]
        [TestCase(3, 2, 3, 0, 2)]
        [TestCase(3, 2, 4, 1, 2)]
        [TestCase(3, 2, 5, 1, 2)]
        [TestCase(3, 2, 5, 2, 2)]
        [TestCase(3, 2, 5, 3, 2)]
        [TestCase(3, 2, 2, 4, 2)]
        [TestCase(3, 2, 3, 4, 2)]
        [TestCase(3, 2, 4, 4, 2)]
        
        [TestCase(0, 0, 5, 5, 8)]
        [TestCase(0, 0, 5, 0, 5)]
        [TestCase(0, 0, 5, 2, 5)]
        public void CalculateDistance_ForTestCases_ReturnsDistance(int xa, int ya, int xb, int yb, int expected)
        {
            var helper = new HexCoordinateHelper();

            var distance = helper.CalculateDistance(
                new HexCoordinate { X = xa, Y = ya }, 
                new HexCoordinate { X = xb, Y = yb });
            
            Assert.AreEqual(expected, distance);
        }
        
        [TestCase(0, 0, 0, 0, true)]
        [TestCase(0, 5, 0, 5, true)]
        [TestCase(3, 0, 3, 0, true)]
        [TestCase(4, 4, 4, 4, true)]
        [TestCase(4, 4, 4, 4, true)]
        [TestCase(0, 0, 1, 1, false)]
        [TestCase(2, 2, 1, 1, false)]
        [TestCase(0, 0, 1, 1, false)]
        [TestCase(2, 0, 0, 2, false)]
        public void SamePoint_ForTestCases_ReturnsEquality (int xa, int ya, int xb, int yb, bool expected)
        {
            var helper = new HexCoordinateHelper();

            var isSamePoint = helper.SamePoint(
                new HexCoordinate { X = xa, Y = ya }, 
                new HexCoordinate { X = xb, Y = yb });
            
            Assert.AreEqual(expected, isSamePoint);
        }
    }
}