using System;

namespace Discoverer.Logic.GameContract
{
    public record Player(int Order, string Name, Guid Id);
}