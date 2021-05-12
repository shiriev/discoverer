using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record PutImproperCellAfterFailCommand(
        Player Player,
        ICoordinate Cell) : GameCommand;
}