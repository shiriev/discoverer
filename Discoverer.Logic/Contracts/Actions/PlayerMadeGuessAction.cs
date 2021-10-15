using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerMadeGuessAction(
        int PlayerNum,
        ICoordinate Cell) : GameAction;
}