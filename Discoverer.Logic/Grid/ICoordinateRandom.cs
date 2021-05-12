
namespace Discoverer.Logic.Grid
{
    public interface ICoordinateRandom<TCoord> where TCoord : notnull
    {
        TCoord Next();
    }
}