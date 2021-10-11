using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record MakeGuessCommand(
        ICoordinate Coordinate) : GameCommand;
}