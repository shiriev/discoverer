using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Logic.Contracts
{
    public abstract record GameCommand
    {
        public abstract EGameCommand Type { get; }
    }
}