using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record PutImproperCellOnStartCommand(
        Player Player,
        ICoordinate Cell) : GameCommand;
}