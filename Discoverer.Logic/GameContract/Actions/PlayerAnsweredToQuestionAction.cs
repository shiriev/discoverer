using System;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Actions
{
    public record PlayerAnsweredToQuestionAction(
        int AskingPlayerNum,
        int AnsweringPlayerNum,
        bool Answer,
        ICoordinate Cell) : GameAction;
}