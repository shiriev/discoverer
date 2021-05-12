using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerMadeGuessAction(
        Player Player,
        ICoordinate Cell) : GameAction;
}