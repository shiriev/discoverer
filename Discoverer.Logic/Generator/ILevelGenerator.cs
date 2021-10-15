using Discoverer.Logic.Contracts;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Generator
{
    internal interface ILevelGenerator
    {
        public Level Generate(GameSettings settings);
    }
}