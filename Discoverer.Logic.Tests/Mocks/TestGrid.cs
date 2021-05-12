using System.Collections.Generic;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Tests.Mocks
{
    public class TestGrid<T> : IGrid<T, TestCoordinate>
    {
        public string Type { get; }
        
        public IEnumerable<(TestCoordinate, T)> Items { get; }
        
        public int Size { get; }
        
        public T Get(TestCoordinate coord)
        {
            throw new System.NotImplementedException();
        }

        public void Set(TestCoordinate coord, T value)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<(TestCoordinate, T)> NearItems(TestCoordinate coord, int distance = 1)
        {
            throw new System.NotImplementedException();
        }
    }
}