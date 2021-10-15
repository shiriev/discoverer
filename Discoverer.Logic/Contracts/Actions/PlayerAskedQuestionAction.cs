using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerAskedQuestionAction(
        int AskingPlayerNum,
        int AnsweringPlayerNum,
        ICoordinate Cell) : GameAction;
}