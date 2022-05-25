using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Logic.Contracts.Actions
{
    public record PlayerWonAction(int PlayerNum) : GameAction
    {
        public override EGameAction Type => EGameAction.PlayerWon;
    }
}