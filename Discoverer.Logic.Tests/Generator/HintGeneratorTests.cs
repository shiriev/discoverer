using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.Generator;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Generator
{
    public class HintGeneratorTests
    {
        public static IEnumerable<TestCaseData> CombineTestCaseData
        {
            get
            {
                yield return new TestCaseData(0, 0, Enumerable.Empty<int[]>());
                yield return new TestCaseData(0, 1, Enumerable.Empty<int[]>());
                yield return new TestCaseData(0, 2, Enumerable.Empty<int[]>());
                yield return new TestCaseData(1, 0, Enumerable.Empty<int[]>());
                yield return new TestCaseData(2, 0, Enumerable.Empty<int[]>());
                yield return new TestCaseData(1, 2, Enumerable.Empty<int[]>());
                yield return new TestCaseData(2, 1, new[] {new [] { 0 }, new [] { 1 } });
                yield return new TestCaseData(1, 1, new[] {new [] { 0 } });
                yield return new TestCaseData(3, 3, new[] {new [] { 0, 1, 2 } });
                yield return new TestCaseData(5, 3, 
                    new[]
                    {
                        new [] { 0, 1, 2 },
                        new [] { 0, 1, 3 },
                        new [] { 0, 1, 4 },
                        new [] { 0, 2, 3 },
                        new [] { 0, 2, 4 },
                        new [] { 0, 3, 4 },
                        new [] { 1, 2, 3 },
                        new [] { 1, 2, 4 },
                        new [] { 1, 3, 4 },
                        new [] { 2, 3, 4 },
                    });
                yield return new TestCaseData(5, 1, 
                    new[]
                    {
                        new [] { 0 },
                        new [] { 1 },
                        new [] { 2 },
                        new [] { 3 },
                        new [] { 4 },
                    });
                yield return new TestCaseData(5, 4, 
                    new[]
                    {
                        new [] { 0, 1, 2, 3 },
                        new [] { 0, 1, 2, 4 },
                        new [] { 0, 1, 3, 4 },
                        new [] { 0, 2, 3, 4 },
                        new [] { 1, 2, 3, 4 },
                    });
            }
        }

        [TestCaseSource(nameof(CombineTestCaseData))]
        public void Combine_ForAllTestCases_ReturnAllSets(int fullSetSize, int count, IEnumerable<int[]> expected)
        {
            var fullSet = Enumerable.Range(0, fullSetSize).ToArray();
            
            var result = HintGenerator.Combine(fullSet, count).ToArray();
            
            CollectionAssert.AreEquivalent(
                expected.Select(a => string.Join(',', a.OrderBy(b => b))),
                result.Select(a => string.Join(',', a.OrderBy(b => b))));
        }
        
        
        
        public static IEnumerable<TestCaseData> GenerateTestData
        {
            get
            {
                yield return new TestCaseData(5, 2, new Dictionary<EHint, int[]>
                    {
                        { EHint.DesertOrForest, Enumerable.Range(0, 5).ToArray() },
                        { EHint.DesertOrMountain, Array.Empty<int>() },
                    },
                    Array.Empty<(TestCoordinate, EHint[])>()
                );
                yield return new TestCaseData(5, 2, new Dictionary<EHint, int[]>
                    {
                        { EHint.DesertOrForest, Array.Empty<int>() },
                        { EHint.DesertOrMountain, Array.Empty<int>() },
                    },
                    Array.Empty<(TestCoordinate, EHint[])>()
                );
                yield return new TestCaseData(5, 2, new Dictionary<EHint, int[]>
                    {
                        { EHint.DesertOrForest, Enumerable.Range(0, 5).ToArray() },
                        { EHint.DesertOrMountain, Enumerable.Range(0, 5).ToArray() },
                    },
                    Array.Empty<(TestCoordinate, EHint[])>()
                );
                yield return new TestCaseData(5, 2, new Dictionary<EHint, int[]>
                    {
                        { EHint.DesertOrForest, new [] { 0 } },
                        { EHint.DesertOrMountain, new [] { 0 } },
                    },
                    new (TestCoordinate, EHint[])[]
                    {
                        (new() { I = 0 }, new [] {EHint.DesertOrForest, EHint.DesertOrMountain })
                    }
                );
                yield return new TestCaseData(5, 2, new Dictionary<EHint, int[]>
                    {
                        { EHint.DesertOrForest, new [] { 0, 1, 2 } },
                        { EHint.DesertOrMountain, new [] { 2, 3, 4 } },
                        { EHint.DesertOrSwamp, new [] { 1, 2, 3 } },
                    },
                    new (TestCoordinate, EHint[])[]
                    {
                        (new() { I = 2 }, new [] {EHint.DesertOrForest, EHint.DesertOrMountain })
                    }
                );
                yield return new TestCaseData(5, 2, new Dictionary<EHint, int[]>
                    {
                        { EHint.DesertOrForest, new [] { 0, 1, 2 } },
                        { EHint.DesertOrMountain, new [] { 2, 3, 4 } },
                        { EHint.DesertOrSwamp, new [] { 1, 2, 3 } },
                        { EHint.DesertOrWater, new [] { 0, 2, 4 } },
                    },
                    new (TestCoordinate, EHint[])[]
                    {
                        (new() { I = 2 }, new [] {EHint.DesertOrForest, EHint.DesertOrMountain }),
                        (new() { I = 2 }, new [] {EHint.DesertOrSwamp, EHint.DesertOrWater })
                    }
                );
            }
        }
        
        [TestCaseSource(nameof(GenerateTestData))]
        public void Generate_ForTestCases_ReturnCorrectCollection(
            int itemCount, int playerCount, Dictionary<EHint, int[]> hintPositions, (TestCoordinate, EHint[])[] expected)
        {
            var gridMock = new Mock<IGrid<Cell>>();
            gridMock.Setup(_ => _.Items)
                .Returns(
                    Enumerable.Range(0, itemCount)
                        .Select(_ => (new TestCoordinate { I = _} as ICoordinate, new Cell(ETerrainType.Desert, EHabitatType.Bear, new Building(EColor.Black, EBuildingType.Monument))))
                        .ToArray());
            
            var functions = hintPositions.ToDictionary<KeyValuePair<EHint, int[]>, EHint, Func<IGrid<Cell>, ICoordinate, bool>>(
                kv => kv.Key,
                kv => ((_, c) => kv.Value.Contains(((TestCoordinate)c).I)));
            var generator = new HintGenerator(playerCount, functions);
            
            var result = generator.Generate(gridMock.Object);

            Assert.AreEqual(expected.Length, result.Length);
            foreach (var (coord, hints) in expected)
            {
                Assert.IsTrue(
                    result.Any(t => Equals(t.Item1, coord) && t.Item2.SequenceEqual(hints)),
                    $"No coord = {coord} and hints = {string.Join(',', hints)}");
            }
        }
    }
}