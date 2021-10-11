using System.Collections.Generic;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process
{
    public interface IProcessUpdater
    {
        (ProcessState, List<GameAction>) RunCommand(ProcessState processState, IGrid<bool[]> possibleCells,
            GameCommand command);
        
        bool GetCommandPossibility(ProcessState processState, IGrid<bool[]> possibleCells,
            GameCommand command);
    }
}