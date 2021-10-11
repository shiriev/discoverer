using System;

namespace Discoverer.Logic.Grid
{
    public interface IGridBuilder
    {
        IGrid<T> BuildGrid<T>();
        ICoordinateRandom BuildRandom(Random random);
        ICoordinateHelper BuildCoordinateHelper();
    }
}