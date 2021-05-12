using System;

namespace Discoverer.Logic.Grid
{
    public interface IGridBuilder<TCoord> where TCoord : ICoordinate
    {
        IGrid<T, TCoord> BuildGrid<T>();
        ICoordinateRandom<TCoord> BuildRandom(Random random);
        ICoordinateHelper<TCoord> BuildCoordinateHelper();
    }
}