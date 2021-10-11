using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerAskedQuestionAction(
        int AskingPlayerNum,
        int AnsweringPlayerNum,
        ICoordinate Cell) : GameAction;
}