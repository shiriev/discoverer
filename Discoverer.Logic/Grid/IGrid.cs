using System.Collections.Generic;

namespace Discoverer.Logic.Grid
{
    // TODO: Try make Grid non generic
    public interface IGrid<T>
    {
        string Type { get; }
    }
    
    public interface IGrid<T, TCoord> : IGrid<T> where TCoord : ICoordinate
    {
        IEnumerable<(TCoord, T)> Items { get; }
        
        int Size { get; }

        T Get(TCoord coord);
        
        void Set(TCoord coord, T value);

        IEnumerable<(TCoord, T)> NearItems(TCoord coord, int distance = 1);
    }
}