using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerMadeGuessAction(
        int PlayerNum,
        ICoordinate Cell) : GameAction
    {
        public override EGameAction Type => EGameAction.PlayerMadeGuess;
    }
}