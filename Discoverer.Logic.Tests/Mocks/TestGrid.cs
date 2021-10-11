using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Grid.Hex;

namespace Discoverer.Logic.Tests.Mocks
{
    public class TestGrid<T> : IGrid<T>
    {
        private readonly int _size;
        private readonly T[] _cells;

        public TestGrid(int size)
        {
            if (size <= 0) throw new ArgumentException($"{nameof(size)} <= 0");
            _size = size;
            _cells = new T[_size];
        }
        
        public TestGrid(T[] cells)
        {
            _size = cells.Length;
            _cells = (T[])cells.Clone();
        }

        public IEnumerable<(ICoordinate, T)> Items => _cells.Select((t, i) => (new TestCoordinate { I = i }, t)).Select(dummy => ((ICoordinate, T)) dummy);

        public int Size => _size;

        public T Get(ICoordinate coord)
        {
            if (coord is not TestCoordinate testCoordinate) throw new ArgumentException(nameof(coord));
                
            return _cells[testCoordinate.I];
        }

        public void Set(ICoordinate coord, T value)
        {
            if (coord is not TestCoordinate testCoordinate) throw new ArgumentException(nameof(coord));
            
            _cells[testCoordinate.I] = value;
        }

        public T[] Cells => _cells;

        public IGrid<T> Copy()
        {
            return new TestGrid<T>(_cells);
        }

        public string Type => "Test";

        public IEnumerable<(ICoordinate, T)> NearItems(ICoordinate coord, int distance = 1)
        {
            throw new System.NotImplementedException();
        }
    }
}