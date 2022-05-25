using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Logic.Contracts.Commands
{
    public record StartGameCommand : GameCommand
    {
        public override EGameCommand Type => EGameCommand.StartGame;
    }
}