namespace Discoverer.Logic.Grid
{
    public interface ICoordinateHelper
    {
        int CalculateDistance(ICoordinate a, ICoordinate b);

        bool SamePoint(ICoordinate a, ICoordinate b);
    }
}