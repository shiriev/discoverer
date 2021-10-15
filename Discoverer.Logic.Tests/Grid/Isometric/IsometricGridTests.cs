using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.Grid.Isometric;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Grid.Isometric
{
    public class IsometricGridTests
    {
        [TestCase(0, 0, true)]
        [TestCase(0, 3, true)]
        [TestCase(4, 0, true)]
        [TestCase(-1, 5, true)]
        [TestCase(6, -1, true)]
        [TestCase(-3, -3, true)]
        [TestCase(0, -3, true)]
        [TestCase(-4, 0, true)]
        [TestCase(4, 1, false)]
        [TestCase(1, 7, false)]
        public void Constructor_WhenSizeIncorrectThrows(int width, int height, bool throws)
        {
            TestDelegate func = () => new IsometricGrid<int>(width, height);

            if (throws)
            {
                Assert.Throws<ArgumentException>(func);
            }
            else
            {
                Assert.DoesNotThrow(func);
            }
        }
        
        public static IEnumerable<TestCaseData> NearItemsTestCaseData
        {
            get
            {
                yield return new TestCaseData(5, 5, 3, 3, 0, new IsometricCoordinate[]
                {
                    new() { X = 3, Y = 3 },
                });
                yield return new TestCaseData(5, 5, 3, 3, 1, new IsometricCoordinate[]
                {
                    new() { X = 2, Y = 2 },
                    new() { X = 2, Y = 3 },
                    new() { X = 2, Y = 4 },
                    new() { X = 3, Y = 2 },
                    new() { X = 3, Y = 3 },
                    new() { X = 3, Y = 4 },
                    new() { X = 4, Y = 2 },
                    new() { X = 4, Y = 3 },
                    new() { X = 4, Y = 4 },
                });
                yield return new TestCaseData(5, 5, 2, 3, 1, new IsometricCoordinate[]
                {
                    new() { X = 1, Y = 2 },
                    new() { X = 1, Y = 3 },
                    new() { X = 1, Y = 4 },
                    new() { X = 2, Y = 2 },
                    new() { X = 2, Y = 3 },
                    new() { X = 2, Y = 4 },
                    new() { X = 3, Y = 2 },
                    new() { X = 3, Y = 3 },
                    new() { X = 3, Y = 4 },
                });
            }
        }

        [TestCaseSource(nameof(NearItemsTestCaseData))]
        public void NearItems_ForTestCases_ReturnsCorrectItems(int width, int height, int x, int y, int distance, IsometricCoordinate[] expected)
        {
            var coordinateHelper = new IsometricCoordinateHelper();
            var grid = new IsometricGrid<int>(width, height);
            var result = grid.NearItems(new IsometricCoordinate{ X = x, Y = y}, distance);

            CollectionAssert.AreEquivalent(
                expected.Select(c => coordinateHelper.GetUniqueCode(c)), 
                result.Select(t => coordinateHelper.GetUniqueCode(t.Item1)));
        }
        
        [TestCase(5, 5)]
        [TestCase(1, 10)]
        [TestCase(5, 1)]
        [TestCase(1, 1)]
        public void Get_AfterSet_ReturnsHisValue(int width, int height)
        {
            var grid = new IsometricGrid<(int, int)>(width, height);
            var result = grid.Items;

            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    grid.Set(new IsometricCoordinate { X = x, Y = y }, (x, y));
                }
            }

            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    Assert.AreEqual((x, y), grid.Get(new IsometricCoordinate { X = x, Y = y }));
                }
            }
        }
        
        [TestCase(5, 5)]
        [TestCase(1, 10)]
        [TestCase(5, 1)]
        [TestCase(1, 1)]
        public void Cells_ForTestCases_ReturnsArray(int width, int height)
        {
            var grid = new IsometricGrid<(int, int)>(width, height);

            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    grid.Set(new IsometricCoordinate { X = x, Y = y }, (x, y));
                }
            }

            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    Assert.AreEqual((x, y), grid.Cells[x, y]);
                }
            }
        }
        
        [TestCase(5, 5)]
        [TestCase(1, 10)]
        [TestCase(5, 1)]
        [TestCase(1, 1)]
        public void Items_ForTestCases_ReturnsCollection(int width, int height)
        {            
            var coordinateHelper = new IsometricCoordinateHelper();
            var grid = new IsometricGrid<(int, int)>(width, height);
            var result = grid.Items;

            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    grid.Set(new IsometricCoordinate { X = x, Y = y }, (x, y));
                }
            }

            var expected = 
                Enumerable.Range(0, width)
                    .SelectMany(x =>
                        Enumerable.Range(0, height)
                        .Select(y => (new IsometricCoordinate {X = x, Y = y}, (x, y)))
                    );
            
            CollectionAssert.AreEquivalent(
                expected.Select(t => (coordinateHelper.GetUniqueCode(t.Item1), t.Item2)), 
                grid.Items.Select(t => (coordinateHelper.GetUniqueCode(t.Item1), t.Item2)));
        }
        
        [TestCase(5, 5)]
        [TestCase(1, 10)]
        [TestCase(5, 1)]
        [TestCase(1, 1)]
        public void Distance_ForTestCases_ReturnsCollection(int width, int height)
        {
            var coordinateHelper = new IsometricCoordinateHelper();
            var grid = new IsometricGrid<(int, int)>(width, height);
            var result = grid.Items;

            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    grid.Set(new IsometricCoordinate { X = x, Y = y }, (x, y));
                }
            }

            var expected = 
                Enumerable.Range(0, width)
                    .SelectMany(x =>
                        Enumerable.Range(0, height)
                            .Select(y => (new IsometricCoordinate {X = x, Y = y}, (x, y)))
                    );
            
            CollectionAssert.AreEquivalent(
                expected.Select(t => (coordinateHelper.GetUniqueCode(t.Item1), t.Item2)), 
                grid.Items.Select(t => (coordinateHelper.GetUniqueCode(t.Item1), t.Item2)));
        }
        
        [TestCase(10, 10, 100)]
        [TestCase(1, 5, 5)]
        [TestCase(5, 1, 5)]
        [TestCase(2, 2, 4)]
        [TestCase(1, 1, 1)]
        public void Size_ForAllTestCases_ReturnsSize(int width, int height, int expected)
        {
            var grid = new IsometricGrid<(int, int)>(width, height);

            var size = grid.Size;
            
            Assert.AreEqual(expected, size);
        }
        
        [Test]
        public void Copy_ForGrid_MakeFullCopy()
        {
            var coordinateHelper = new IsometricCoordinateHelper();
            var grid = new IsometricGrid<(int, int)>(5, 5);

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    grid.Set(new IsometricCoordinate { X = x, Y = y }, (x, y));
                }
            }

            var newGrid = grid.Copy();
            
            Assert.IsInstanceOf(typeof(IsometricGrid<(int, int)>), newGrid);
            CollectionAssert.AreEqual(
                grid.Items.Select(t => (coordinateHelper.GetUniqueCode(t.Item1), t.Item2)), 
                newGrid.Items.Select(t => (coordinateHelper.GetUniqueCode(t.Item1), t.Item2)));
        }
        
        [Test]
        public void Copy_AfterOriginalWasChanged_CopyIsNotEqualToOriginal()
        {
            var grid = new IsometricGrid<int>(5, 5);

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    grid.Set(new IsometricCoordinate { X = x, Y = y }, 0);
                }
            }

            var newGrid = grid.Copy();
            
            grid.Set(new IsometricCoordinate { X = 1, Y = 1}, 1);
            
            Assert.IsInstanceOf(typeof(IsometricGrid<int>), newGrid);
            CollectionAssert.AreNotEqual(grid.Items, newGrid.Items);
        }
    }
}