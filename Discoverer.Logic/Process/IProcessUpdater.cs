using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Process.Contracts;

namespace Discoverer.Logic.Process
{
    internal interface IProcessUpdater
    {
        (ProcessState, ImmutableList<GameAction>) RunCommand(ProcessState processState, IGrid<bool[]> possibleCells,
            GameCommand command);
        
        bool GetCommandPossibility(ProcessState processState, IGrid<bool[]> possibleCells,
            GameCommand command);
    }
}