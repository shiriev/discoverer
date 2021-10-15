using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Commands
{
    public record MakeGuessCommand(
        ICoordinate Coordinate) : GameCommand;
}