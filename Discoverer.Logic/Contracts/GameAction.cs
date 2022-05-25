using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Logic.Contracts
{
    public abstract record GameAction
    {
        public abstract EGameAction Type { get; }
    }
}