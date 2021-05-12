using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerAskedGuessAction(
        Player AskingPlayer,
        Player AnsweringPlayer,
        ICoordinate Cell) : GameAction;
}