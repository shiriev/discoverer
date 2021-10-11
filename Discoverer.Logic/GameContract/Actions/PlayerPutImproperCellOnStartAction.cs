using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerPutImproperCellOnStartAction(
        int PlayerNum,
        ICoordinate Cell) : GameAction;
}