using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record AskQuestionCommand(
        Player AskingPlayer,
        Player AnsweringPlayer,
        ICoordinate Cell) : GameCommand;
}