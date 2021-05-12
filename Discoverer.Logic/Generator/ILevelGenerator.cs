using Discoverer.Logic.GameContract;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Generator
{
    public interface ILevelGenerator
    {
        public Level Generate(GenerationSettings settings);
    }
}