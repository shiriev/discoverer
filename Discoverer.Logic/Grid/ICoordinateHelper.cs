namespace Discoverer.Logic.Grid
{
    public interface ICoordinateHelper<TCoord> where TCoord : ICoordinate
    {
        int CalculateDistance(TCoord a, TCoord b);
    }
}