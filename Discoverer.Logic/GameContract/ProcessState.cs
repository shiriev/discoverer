using System;
using System.Collections.Immutable;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract
{
    //TODO: Rename class
    public record ProcessState(
        ImmutableList<GameAction> Actions,
        IImmutableGrid<MarkerSet> MarkerSetGrid,
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