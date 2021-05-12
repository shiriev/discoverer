using System;
using System.Collections.Generic;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract
{
    public record ProcessState(
        List<GameAction> Actions,
        IGrid<CellState> CellStateGrid,
        List<Player> Players,
        Guid CurrentPlayerId,
        int CurrentTurn,
        GameState GameState,
        Guid GameId)
    {

        public ProcessState Set(ProcessUpdate update)
        {
            return new ProcessState
            (
                Actions: update.Actions ?? Actions,
                CellStateGrid: update.CellStateGrid ?? CellStateGrid,
                Players: update.Players ?? Players,
                CurrentPlayerId: update.CurrentPlayerId ?? CurrentPlayerId,
                CurrentTurn: update.CurrentTurn ?? CurrentTurn,
                GameState: update.GameState ?? GameState,
                GameId: update.GameId ?? GameId
            );
        }
    }
}