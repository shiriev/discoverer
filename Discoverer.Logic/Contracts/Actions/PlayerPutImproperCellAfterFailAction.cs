using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerPutImproperCellAfterFailAction(
        int PlayerNum,
        ICoordinate Cell
    ) : GameAction;
}