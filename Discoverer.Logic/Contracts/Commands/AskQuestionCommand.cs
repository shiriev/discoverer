using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Commands
{
    public record AskQuestionCommand(
        int AnsweringPlayerNum,
        ICoordinate Coordinate) : GameCommand;
}