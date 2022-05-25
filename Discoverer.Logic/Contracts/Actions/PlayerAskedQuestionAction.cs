using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerAskedQuestionAction(
        int AskingPlayerNum,
        int AnsweringPlayerNum,
        ICoordinate Cell) : GameAction
    {
        public override EGameAction Type => EGameAction.PlayerAskedQuestion;
    }
}