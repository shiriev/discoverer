using System.Collections.Generic;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process
{
    public interface IGameProcess
    {
        ProcessState SaveState();

        void LoadState(ProcessState processState);
        
        List<GameAction> RunCommand(GameCommand command);
        
        IGrid<bool, ICoordinate> GetPossibleCellsFor(Player player);
        
        GameState GetGameState();
        
        List<GameAction> GetAllActions();
        
        IGrid<(Cell, CellState), ICoordinate> GetGrid();
        
        Player GetCurrentPlayer();
        
        int GetCurrentTurn();
        
        List<Player> GetAllPlayers();
        
        List<GameCommand> GetCurrentPossibleCommands();
    }
}