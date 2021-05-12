using System;
using System.Collections.Generic;

namespace Discoverer.Logic.Grid.Isometric
{
    public class IsometricGrid
    {
        public static string GlobalType => "Isometric";
    }
    
    public class IsometricGrid<T> : IsometricGrid, IGrid<T, IsometricCoordinate>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly T[,] _cells;

        public IsometricGrid(int width, int height)
        {
            if (width <= 0) throw new ArgumentException($"{nameof(width)} <= 0");
            if (height <= 0) throw new ArgumentException($"{nameof(height)} <= 0");
            _width = width;
            _height = height;
            _cells = new T[_width, _height];
        }

        public IEnumerable<(IsometricCoordinate, T)> Items
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

        public T Get(IsometricCoordinate coord)
        {
            return _cells[coord.X, coord.Y];
        }

        public void Set(IsometricCoordinate coord, T value)
        {
            _cells[coord.X, coord.Y] = value;
        }

        public IEnumerable<(IsometricCoordinate, T)> NearItems(IsometricCoordinate coord, int distance = 1)
        {
            var minx = Math.Max(0, coord.X - distance);
            var maxx = Math.Min(_width - 1, coord.X + distance);
            var miny = Math.Max(0, coord.Y - distance);
            var maxy = Math.Min(_height - 1, coord.Y + distance);
            for (var x = minx; x <= maxx; ++x)
            {
                for (var y = miny; y <= maxy; ++y)
                {
                    yield return (new() { X = x, Y = y }, _cells[x, y]);
                }
            }
        }

        public T[,] Cells => _cells;

        public string Type => GlobalType;
    }
}