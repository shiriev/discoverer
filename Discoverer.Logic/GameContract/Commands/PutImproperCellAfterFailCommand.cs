using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record PutImproperCellAfterFailCommand(
        ICoordinate Coordinate) : GameCommand;
}