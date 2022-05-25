using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Process.Contracts;

namespace Discoverer.Logic.Process
{
    internal interface IGameStateUpdater
    {
        (GameCast, ImmutableList<GameAction>) RunCommand(GameCast gameCast, IGrid<bool[]> possibleCells, GameCommand command);
        
        bool GetCommandPossibility(GameCast gameCast, IGrid<bool[]> possibleCells, GameCommand command);

        List<(EGameCommand CommandType, List<GameCommand> Commands)> GetCurrentPossibleCommands(GameCast gameCast, IGrid<bool[]> possibleCells);
    }
}