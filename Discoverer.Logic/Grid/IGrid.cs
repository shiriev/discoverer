using System.Collections.Generic;

namespace Discoverer.Logic.Grid
{
    public interface IGrid<T>
    {
        string Type { get; }
        
        IEnumerable<(ICoordinate, T)> Items { get; }
        
        int Size { get; }

        T Get(ICoordinate coord);
        
        void Set(ICoordinate coord, T value);

        IEnumerable<(ICoordinate, T)> NearItems(ICoordinate coord, int distance = 1);

        IGrid<T> Copy();
    }
}