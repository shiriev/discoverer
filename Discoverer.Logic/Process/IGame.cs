﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process
{
    public interface IGame
    {
        GameCast SaveState();

        void LoadState(GameCast gameCast);
        
        ImmutableList<GameAction> RunCommand(GameCommand command);
        
        IGrid<bool> GetPossibleCellsFor(int playerNum);
        
        GameState GetGameState();
        
        ImmutableList<GameAction> GetAllActions();
        
        int GetCurrentPlayer();
        
        int GetCurrentTurn();
        
        List<Type> GetCurrentPossibleCommands();
    }
}