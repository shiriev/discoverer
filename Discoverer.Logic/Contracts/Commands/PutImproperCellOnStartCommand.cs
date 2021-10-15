using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Commands
{
    public record PutImproperCellOnStartCommand(
        ICoordinate Coordinate) : GameCommand;
}