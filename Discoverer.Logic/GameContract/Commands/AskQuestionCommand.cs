using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record AskQuestionCommand(
        int AnsweringPlayerNum,
        ICoordinate Coordinate) : GameCommand;
}