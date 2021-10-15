using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerPutImproperCellOnStartAction(
        int PlayerNum,
        ICoordinate Cell) : GameAction;
}