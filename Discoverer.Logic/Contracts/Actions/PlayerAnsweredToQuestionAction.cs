using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerAnsweredToQuestionAction(
        int AskingPlayerNum,
        int AnsweringPlayerNum,
        bool Answer,
        ICoordinate Cell) : GameAction;
}