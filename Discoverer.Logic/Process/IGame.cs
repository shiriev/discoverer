using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process
{
    public interface IGame
    {
        GameSave SaveState();

        void LoadState(GameSave gameSave);
        
        ImmutableList<GameAction> RunCommand(GameCommand command);
        
        IGrid<bool> GetPossibleCellsFor(int playerNum);
        
        GameState GetGameState();
        
        ImmutableList<GameAction> GetAllActions();
        
        int GetCurrentPlayer();
        
        int GetCurrentTurn();
        
        List<(EGameCommand CommandType, List<GameCommand> Commands)> GetCurrentPossibleCommands();
    }
}