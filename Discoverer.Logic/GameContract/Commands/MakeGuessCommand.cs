using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record MakeGuessCommand(
        Player Player,
        ICoordinate Cell) : GameCommand;
}