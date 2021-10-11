using System;
using System.Collections.Generic;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract
{
    //TODO: Make ProcessState full immutable
    //TODO: Rename class
    public record ProcessState(
        List<GameAction> Actions,
        IGrid<MarkerSet> MarkerSetGrid,
        int CurrentPlayerNum,
        int CurrentTurn,
        GameState GameState,
        int PlayerCount,
        Guid GameId)
    {

        public ProcessState Set(ProcessUpdate update)
        {
            return new
            (
                Actions: update.Actions ?? Actions,
                MarkerSetGrid: update.MarkerSetGrid ?? MarkerSetGrid,
                CurrentPlayerNum: update.CurrentPlayerNum ?? CurrentPlayerNum,
                CurrentTurn: update.CurrentTurn ?? CurrentTurn,
                GameState: update.GameState ?? GameState,
                GameId: GameId,
                PlayerCount: PlayerCount
            );
        }
    }
}