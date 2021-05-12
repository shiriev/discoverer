using System;
using System.Collections.Generic;

namespace Discoverer.Logic.Grid.Hex
{
    public class HexGrid
    {
        public static string GlobalType => "Hex";
    }
    
    public class HexGrid<T> : HexGrid, IGrid<T, HexCoordinate>
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

        public IEnumerable<(HexCoordinate, T)> Items
        {
            get
            {
                for (var x = 0; x < _cells.GetLength(0); ++x)
                {
                    for (var y = 0; y < _cells.GetLength(1); ++y)
                    {
                        yield return (new() { X = x, Y = y }, _cells[x, y]);
                    }
                }
            }
        }

        public int Size => _width * _height;

        public T Get(HexCoordinate coord)
        {
            return _cells[coord.X, coord.Y];
        }

        public void Set(HexCoordinate coord, T value)
        {
            _cells[coord.X, coord.Y] = value;
        }

        public T[,] Cells => _cells;

        public IEnumerable<(HexCoordinate, T)> NearItems(HexCoordinate coord, int distance = 1)
        {
            // TODO: Описать алгоритм
            for (var diffx = -distance; diffx <= distance; ++diffx)
            {
                var x = coord.X + diffx;
                if (x < 0)
                    continue;
                if (x >= _width)
                    break;
                var ylen = distance * 2 + 1 - Math.Abs(diffx);
                var miny = coord.Y - ylen / 2;

                if (Math.Abs(diffx) % 2 == 1 && coord.X % 2 == 1)
                    miny++;
                
                var maxy = miny + ylen - 1;
                
                miny = Math.Max(0, miny);
                maxy = Math.Min(_height - 1, maxy);
                for (var y = miny; y <= maxy; ++y)
                {
                    yield return (new() { X = x, Y = y }, _cells[x, y]);
                }
            }
        }

        public string Type => GlobalType;
    }
}