using Discoverer.Logic.Grid.Isometric;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Grid.Isometric
{
    public class IsometricCoordinateHelperTests
    {
        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(0, 0, 3, 0, 3)]
        [TestCase(0, 0, 0, 4, 4)]
        [TestCase(0, 0, 3, 3, 3)]
        [TestCase(2, 2, 4, 8, 6)]
        [TestCase(4, 8, 2, 2, 6)]
        [TestCase(9, 5, 2, 1, 7)]
        public void Distance_ForTestCases_ReturnsDistance(int xa, int ya, int xb, int yb, int expected)
        {
            var helper = new IsometricCoordinateHelper();

            var distance = helper.CalculateDistance(
                new IsometricCoordinate { X = xa, Y = ya }, 
                new IsometricCoordinate { X = xb, Y = yb });
            
            Assert.AreEqual(expected, distance);
        }
    }
}