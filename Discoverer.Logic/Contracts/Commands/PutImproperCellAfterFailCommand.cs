using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Contracts.Commands
{
    public record PutImproperCellAfterFailCommand(
        ICoordinate Coordinate) : GameCommand
    {
        public override EGameCommand Type => EGameCommand.PutImproperCellAfterFail;
    }
}