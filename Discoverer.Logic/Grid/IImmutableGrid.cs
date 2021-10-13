using System.Collections.Generic;

namespace Discoverer.Logic.Grid
{
    public interface IImmutableGrid<T>
    {
        string Type { get; }
        
        IEnumerable<(ICoordinate, T)> Items { get; }
        
        int Size { get; }

        T Get(ICoordinate coord);

        IEnumerable<(ICoordinate, T)> NearItems(ICoordinate coord, int distance = 1);

        IGrid<T> CopyGrid();

        IImmutableGrid<T> CopyWithSet(ICoordinate coord, T value);
    }
}