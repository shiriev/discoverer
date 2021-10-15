using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Commands
{
    public record PutImproperCellAfterFailCommand(
        ICoordinate Coordinate) : GameCommand;
}