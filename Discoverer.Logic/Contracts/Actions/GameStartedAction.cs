using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Logic.Contracts.Actions
{
    public record GameStartedAction : GameAction
    {
        public override EGameAction Type => EGameAction.GameStarted;
    }
}