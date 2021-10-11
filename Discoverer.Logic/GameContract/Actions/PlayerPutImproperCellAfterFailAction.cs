using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerPutImproperCellAfterFailAction(
        int PlayerNum,
        ICoordinate Cell
    ) : GameAction;
}