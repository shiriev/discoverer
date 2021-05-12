using System;
using System.Collections.Generic;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process
{
    public class GameProcess : IGameProcess
    {
        private readonly Level _level;
        
        public GameProcess(Level level)
        {
            _level = level;
        }

        public ProcessState SaveState()
        {
            throw new NotImplementedException();
        }

        public void LoadState(ProcessState processState)
        {
            throw new NotImplementedException();
        }

        public List<GameAction> RunCommand(GameCommand command)
        {
            throw new System.NotImplementedException();
        }

        public IGrid<bool, ICoordinate> GetPossibleCellsFor(Player player)
        {
            throw new System.NotImplementedException();
        }

        public GameState GetGameState()
        {
            throw new System.NotImplementedException();
        }

        public List<GameAction> GetAllActions()
        {
            throw new System.NotImplementedException();
        }

        public IGrid<(Cell, CellState), ICoordinate> GetGrid()
        {
            throw new System.NotImplementedException();
        }

        public Player GetCurrentPlayer()
        {
            throw new System.NotImplementedException();
        }

        public List<Player> GetAllPlayers()
        {
            throw new System.NotImplementedException();
        }

        public List<GameCommand> GetCurrentPossibleCommands()
        {
            throw new System.NotImplementedException();
        }

        public int GetCurrentTurn()
        {
            throw new System.NotImplementedException();
        }
    }
}