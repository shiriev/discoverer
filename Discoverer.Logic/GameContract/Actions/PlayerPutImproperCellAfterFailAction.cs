using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerPutImproperCellAfterFailAction(
        Player Player,
        ICoordinate Cell
    ) : GameAction;
}