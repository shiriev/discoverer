using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Commands
{
    public record PutImproperCellOnStartCommand(
        ICoordinate Coordinate) : GameCommand
    {
        public override EGameCommand Type => EGameCommand.PutImproperCellOnStart;
    }
}