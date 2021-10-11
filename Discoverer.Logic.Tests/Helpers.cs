using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Tests
{
    public static class Helpers
    {
        public static IGrid<T> CopyWithSet<T>(IGrid<T> grid, ICoordinate coord, T value)
        {
            var newGrid = grid.Copy();
            newGrid.Set(coord, value);
            return newGrid;
        }
    }
}