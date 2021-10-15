using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Process.Contracts;

namespace Discoverer.Logic.Process
{
    internal interface IProcessUpdater
    {
        (GameCast, ImmutableList<GameAction>) RunCommand(GameCast gameCast, IGrid<bool[]> possibleCells,
            GameCommand command);
        
        bool GetCommandPossibility(GameCast gameCast, IGrid<bool[]> possibleCells,
            GameCommand command);
    }
}