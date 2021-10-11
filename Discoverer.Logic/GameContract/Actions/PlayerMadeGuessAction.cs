using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerMadeGuessAction(
        int PlayerNum,
        ICoordinate Cell) : GameAction;
}