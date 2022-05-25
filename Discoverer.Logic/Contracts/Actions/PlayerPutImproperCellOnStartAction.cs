using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerPutImproperCellOnStartAction(
        int PlayerNum,
        ICoordinate Cell) : GameAction
    {
        public override EGameAction Type => EGameAction.PlayerPutImproperCellOnStart;
    }
}