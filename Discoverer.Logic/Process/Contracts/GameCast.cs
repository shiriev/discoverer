using System;
using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process.Contracts
{
    internal record GameCast(
        ImmutableList<GameAction> Actions,
        IImmutableGrid<MarkerSet> MarkerSetGrid,
        int CurrentPlayerNum,
        int CurrentTurn,
        GameState GameState,
        int PlayerCount,
        Guid GameId)
    {

        public GameCast Update(UpdateGameCastRequest updateRequest)
        {
            return new
            (
                Actions: updateRequest.Actions ?? Actions,
                MarkerSetGrid: updateRequest.MarkerSetGrid ?? MarkerSetGrid,
                CurrentPlayerNum: updateRequest.CurrentPlayerNum ?? CurrentPlayerNum,
                CurrentTurn: updateRequest.CurrentTurn ?? CurrentTurn,
                GameState: updateRequest.GameState ?? GameState,
                GameId: GameId,
                PlayerCount: PlayerCount
            );
        }
    }
}