﻿using System;
using System.Collections.Generic;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process
{
    public interface IGameProcess
    {
        GameCast SaveState();

        void LoadState(GameCast gameCast);
        
        List<GameAction> RunCommand(GameCommand command);
        
        IGrid<bool> GetPossibleCellsFor(int playerNum);
        
        GameState GetGameState();
        
        List<GameAction> GetAllActions();
        
        int GetCurrentPlayer();
        
        int GetCurrentTurn();
        
        List<Type> GetCurrentPossibleCommands();
    }
}