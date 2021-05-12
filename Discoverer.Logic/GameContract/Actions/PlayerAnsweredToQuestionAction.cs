using System;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerAnsweredToQuestionAction(
        Player AskingPlayer,
        Player AnsweringPlayer,
        bool Answer,
        ICoordinate Cell) : GameAction;
}