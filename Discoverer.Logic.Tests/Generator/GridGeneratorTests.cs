using System;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.Generator;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Generator
{
    public class GridGeneratorTests
    {
        [Test]
        public void Generate_DoesntThrow()
        {
            var coordinateRandomMock = new Mock<ICoordinateRandom<TestCoordinate>>();
            coordinateRandomMock.SetupSequence(_ => _.Next())
                .Returns(new TestCoordinate { I = 0 })
                .Returns(new TestCoordinate { I = 1 })
                .Returns(new TestCoordinate { I = 2 })
                .Returns(new TestCoordinate { I = 3 })
                .Returns(new TestCoordinate { I = 4 })
                .Returns(new TestCoordinate { I = 5 })
                .Returns(new TestCoordinate { I = 6 })
                .Returns(new TestCoordinate { I = 7 })
                .Returns(new TestCoordinate { I = 8 })
                .Returns(new TestCoordinate { I = 9 })
                .Returns(new TestCoordinate { I = 0 })
                .Returns(new TestCoordinate { I = 1 })
                .Returns(new TestCoordinate { I = 2 })
                .Returns(new TestCoordinate { I = 3 })
                .Returns(new TestCoordinate { I = 4 })
                .Returns(new TestCoordinate { I = 5 })
                .Returns(new TestCoordinate { I = 6 })
                .Returns(new TestCoordinate { I = 7 })
                .Returns(new TestCoordinate { I = 8 })
                .Returns(new TestCoordinate { I = 9 });
            var coordinateHelperMock = new Mock<ICoordinateHelper<TestCoordinate>>();
            coordinateHelperMock.Setup(_ => _.CalculateDistance(It.IsAny<TestCoordinate>(), It.IsAny<TestCoordinate>())).Returns(1);
            var cellGridMock = new Mock<IGrid<Cell, TestCoordinate>>();
            cellGridMock.Setup(_ => _.Items).Returns(Enumerable.Empty<(TestCoordinate, Cell)>());
            cellGridMock.Setup(_ => _.Size).Returns(10);
            cellGridMock.Setup(_ => _.Get(It.IsAny<TestCoordinate>())).Returns(new Cell());
            cellGridMock.Setup(_ => _.Set(It.IsAny<TestCoordinate>(), It.IsAny<Cell>()));
            cellGridMock.Setup(_ => _.NearItems(It.IsAny<TestCoordinate>(), It.IsAny<int>())).Returns(Enumerable.Empty<(TestCoordinate, Cell)>());
            var terrainGridMock = new Mock<IGrid<ETerrainType, TestCoordinate>>();
            terrainGridMock.Setup(_ => _.Items).Returns(Enumerable.Repeat((new TestCoordinate { I = 0 }, ETerrainType.Desert), 10));
            terrainGridMock.Setup(_ => _.Size).Returns(10);
            terrainGridMock.Setup(_ => _.Get(It.IsAny<TestCoordinate>())).Returns(ETerrainType.Desert);
            terrainGridMock.Setup(_ => _.Set(It.IsAny<TestCoordinate>(), It.IsAny<ETerrainType>()));
            terrainGridMock.Setup(_ => _.NearItems(It.IsAny<TestCoordinate>(), It.IsAny<int>())).Returns(Enumerable.Empty<(TestCoordinate, ETerrainType)>());
            var habitatGridMock = new Mock<IGrid<EHabitatType?, TestCoordinate>>();
            habitatGridMock.Setup(_ => _.Items).Returns(Enumerable.Repeat((new TestCoordinate { I = 0 }, (EHabitatType?)EHabitatType.Bear), 10));
            habitatGridMock.Setup(_ => _.Size).Returns(10);
            habitatGridMock.Setup(_ => _.Get(It.IsAny<TestCoordinate>())).Returns((EHabitatType?)null);
            habitatGridMock.Setup(_ => _.Set(It.IsAny<TestCoordinate>(), It.IsAny<EHabitatType?>()));
            habitatGridMock.Setup(_ => _.NearItems(It.IsAny<TestCoordinate>(), It.IsAny<int>())).Returns(Enumerable.Empty<(TestCoordinate, EHabitatType?)>());

            // TODO: Implement mock object for IGrid
            var builderMock = new Mock<IGridBuilder<TestCoordinate>>();
            builderMock.Setup(_ => _.BuildRandom(It.IsAny<Random>())).Returns(coordinateRandomMock.Object);
            builderMock.Setup(_ => _.BuildCoordinateHelper()).Returns(coordinateHelperMock.Object);
            builderMock.Setup(_ => _.BuildGrid<EHabitatType?>()).Returns(habitatGridMock.Object);
            builderMock.Setup(_ => _.BuildGrid<ETerrainType>()).Returns(terrainGridMock.Object);
            builderMock.Setup(_ => _.BuildGrid<Cell>()).Returns(cellGridMock.Object);

            var generator = new GridGenerator<TestCoordinate>(builderMock.Object, new Random());

            var grid = generator.Generate();
            
            Assert.Pass();
        }
    }
}