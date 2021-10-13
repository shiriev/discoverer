using System.Collections.Generic;

namespace Discoverer.Logic.Grid
{
    public class ImmutableGrid<T> : IImmutableGrid<T>
    {
        public static ImmutableGrid<T> FromGrid(IGrid<T> grid)
        {
            return new(grid);
        }

        private readonly IGrid<T> _grid;
        
        private ImmutableGrid(IGrid<T> grid)
        {
            _grid = grid;
        }

        public string Type => _grid.Type;

        public IEnumerable<(ICoordinate, T)> Items => _grid.Items;

        public int Size => _grid.Size;
        
        public T Get(ICoordinate coord)
        {
            return _grid.Get(coord);
        }

        public IImmutableGrid<T> CopyWithSet(ICoordinate coord, T value)
        {
            var newGrid = _grid.Copy();
            newGrid.Set(coord, value);
            return FromGrid(newGrid);
        }

        public IEnumerable<(ICoordinate, T)> NearItems(ICoordinate coord, int distance = 1)
        {
            return _grid.NearItems(coord, distance);
        }

        public IGrid<T> CopyGrid()
        {
            return _grid.Copy();
        }
    }

    public static class ImmutableGridExtensions
    {
        public static ImmutableGrid<T> ToImmutable<T>(this IGrid<T> grid) => ImmutableGrid<T>.FromGrid(grid);
    }
}