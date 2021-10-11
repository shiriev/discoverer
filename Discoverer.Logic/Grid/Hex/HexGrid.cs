using System;
using System.Collections.Generic;

namespace Discoverer.Logic.Grid.Hex
{
    public class HexGrid
    {
        public static string GlobalType => "Hex";
    }
    
    public class HexGrid<T> : HexGrid, IGrid<T>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly T[,] _cells;

        public HexGrid(int width, int height)
        {
            if (width <= 0) throw new ArgumentException($"{nameof(width)} <= 0");
            if (height <= 0) throw new ArgumentException($"{nameof(height)} <= 0");
            _width = width;
            _height = height;
            _cells = new T[_width, _height];
        }
        
        private HexGrid(T[,] cells)
        {
            _width = cells.GetLength(0);
            _height = cells.GetLength(1);
            _cells = (T[,]) cells.Clone();
        }

        public IEnumerable<(ICoordinate, T)> Items
        {
            get
            {
                for (var x = 0; x < _cells.GetLength(0); ++x)
                {
                    for (var y = 0; y < _cells.GetLength(1); ++y)
                    {
                        yield return (new HexCoordinate() { X = x, Y = y }, _cells[x, y]);
                    }
                }
            }
        }

        public int Size => _width * _height;

        public T Get(ICoordinate coord)
        {
            if (coord is not HexCoordinate hexCoordinate) throw new ArgumentException(nameof(coord));
                
            return _cells[hexCoordinate.X, hexCoordinate.Y];
        }

        public void Set(ICoordinate coord, T value)
        {
            if (coord is not HexCoordinate hexCoordinate) throw new ArgumentException(nameof(coord));
            
            _cells[hexCoordinate.X, hexCoordinate.Y] = value;
        }

        public T[,] Cells => _cells;

        public IEnumerable<(ICoordinate, T)> NearItems(ICoordinate coord, int distance = 1)
        {
            if (coord is not HexCoordinate hexCoordinate) throw new ArgumentException(nameof(coord));
            
            // TODO: Описать алгоритм
            for (var diffx = -distance; diffx <= distance; ++diffx)
            {
                var x = hexCoordinate.X + diffx;
                if (x < 0)
                    continue;
                if (x >= _width)
                    break;
                var ylen = distance * 2 + 1 - Math.Abs(diffx);
                var miny = hexCoordinate.Y - ylen / 2;

                if (Math.Abs(diffx) % 2 == 1 && hexCoordinate.X % 2 == 1)
                    miny++;
                
                var maxy = miny + ylen - 1;
                
                miny = Math.Max(0, miny);
                maxy = Math.Min(_height - 1, maxy);
                for (var y = miny; y <= maxy; ++y)
                {
                    yield return (new HexCoordinate() { X = x, Y = y }, _cells[x, y]);
                }
            }
        }

        public IGrid<T> Copy()
        {
            return new HexGrid<T>(_cells);
        }

        public string Type => GlobalType;
    }
}