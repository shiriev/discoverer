using System.Linq;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Tests.Mocks;
using Moq;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Grid
{
    public class ImmutableGridTests
    {
        [Test]
        public void FromGrid_WithGrid_CreateFullCopy()
        {
            var gridMock = new Mock<IGrid<int>>();
            var items = Enumerable.Range(0, 5).Select(i => (new TestCoordinate {I = i} as ICoordinate, i)).ToArray();
            var size = 0;
            var type = "test";
            var getValue = -1;
            var nearItemsValue = Enumerable.Range(1, 3).Select(i => (new TestCoordinate {I = i} as ICoordinate, i)).ToArray();
            var copyValue = new TestGrid<int>(5);
            
            gridMock.Setup(gr => gr.Items).Returns(items);
            gridMock.Setup(gr => gr.Size).Returns(size);
            gridMock.Setup(gr => gr.Type).Returns(type);
            gridMock.Setup(gr => gr.Get(It.IsAny<ICoordinate>())).Returns(getValue);
            gridMock.Setup(gr => gr.NearItems(It.IsAny<ICoordinate>(), It.IsAny<int>())).Returns(nearItemsValue);
            gridMock.Setup(gr => gr.Copy()).Returns(copyValue);

            var immutableGrid = ImmutableGrid<int>.FromGrid(gridMock.Object);

            Assert.AreEqual(items, immutableGrid.Items);
            Assert.AreEqual(size, immutableGrid.Size);
            Assert.AreEqual(type, immutableGrid.Type);
            Assert.AreEqual(getValue, immutableGrid.Get(new TestCoordinate { I = 0 }));
            Assert.AreEqual(nearItemsValue, immutableGrid.NearItems( new TestCoordinate { I = 0 }));
            Assert.AreEqual(copyValue, immutableGrid.CopyGrid());
        }
        
        [Test]
        public void CopyWithSet_WithGrid_CreateCopy()
        {
            var gridMock = new Mock<IGrid<int>>();
            var copyMock = new Mock<IGrid<int>>();
            var copyItems = Enumerable.Range(0, 5).Select(i => (new TestCoordinate {I = i} as ICoordinate, i)).ToArray();
            gridMock.Setup(gr => gr.Copy()).Returns(copyMock.Object);
            copyMock.Setup(gr => gr.Items).Returns(copyItems);

            var immutableGrid = ImmutableGrid<int>.FromGrid(gridMock.Object).CopyWithSet(new TestCoordinate { I = 0 }, 0);

            copyMock.Verify(gr => gr.Set(It.IsAny<ICoordinate>(), It.IsAny<int>()));
            Assert.AreEqual(copyItems, immutableGrid.Items);
        }
    }
}