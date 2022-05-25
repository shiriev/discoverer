using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerAnsweredToGuessAction(
        int AskingPlayerNum,
        int AnsweringPlayerNum,
        bool Answer,
        ICoordinate Cell) : GameAction
    {
        public override EGameAction Type => EGameAction.PlayerAnsweredToGuess;
    }
}