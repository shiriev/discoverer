using System;
using System.Collections.Generic;

namespace Discoverer.Logic.Grid.Isometric
{
    public class IsometricGrid
    {
        public static string GlobalType => "Isometric";
    }
    
    public class IsometricGrid<T> : IsometricGrid, IGrid<T>
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
        
        private IsometricGrid(T[,] cells)
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
                        yield return (new IsometricCoordinate() { X = x, Y = y }, _cells[x, y]);
                    }
                }
            }
        }

        public int Size => _width * _height;

        public T Get(ICoordinate coord)
        {
            if (coord is not IsometricCoordinate isometricCoordinate) throw new ArgumentException(nameof(coord));

            return _cells[isometricCoordinate.X, isometricCoordinate.Y];
        }

        public void Set(ICoordinate coord, T value)
        {
            if (coord is not IsometricCoordinate isometricCoordinate) throw new ArgumentException(nameof(coord));
            
            _cells[isometricCoordinate.X, isometricCoordinate.Y] = value;
        }

        public IEnumerable<(ICoordinate, T)> NearItems(ICoordinate coord, int distance = 1)
        {
            if (coord is not IsometricCoordinate isometricCoordinate) throw new ArgumentException(nameof(coord));
            
            var minx = Math.Max(0, isometricCoordinate.X - distance);
            var maxx = Math.Min(_width - 1, isometricCoordinate.X + distance);
            var miny = Math.Max(0, isometricCoordinate.Y - distance);
            var maxy = Math.Min(_height - 1, isometricCoordinate.Y + distance);
            for (var x = minx; x <= maxx; ++x)
            {
                for (var y = miny; y <= maxy; ++y)
                {
                    yield return (new IsometricCoordinate() { X = x, Y = y }, _cells[x, y]);
                }
            }
        }

        public IGrid<T> Copy()
        {
            return new IsometricGrid<T>(_cells);
        }

        public T[,] Cells => _cells;

        public string Type => GlobalType;
    }
}