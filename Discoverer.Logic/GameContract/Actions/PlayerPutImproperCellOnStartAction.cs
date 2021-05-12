using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerPutImproperCellOnStartAction(
        Player Player,
        ICoordinate Cell) : GameAction;
}