using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerAnsweredToGuessAction(
        int AskingPlayerNum,
        int AnsweringPlayerNum,
        bool Answer,
        ICoordinate Cell) : GameAction;
}