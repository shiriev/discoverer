using System;
using System.Linq;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Generator
{
    internal sealed class LevelGenerator : ILevelGenerator
    {
        private const int AttemptMaxCount = 20;
        
        public Level Generate(GameSettings settings)
        {
            if (settings.PlayerCount <= 1)
                throw new ArgumentOutOfRangeException(nameof(settings.PlayerCount), settings.PlayerCount, null);
            
            var random = settings.Seed.HasValue ? new Random(settings.Seed.Value) : new Random();
            
            IGridBuilder gridBuilder = 
                settings.GridType == EGridType.Hex 
                    ? new HexBuilder(settings.Width, settings.Height) 
                    : new IsometricBuilder(settings.Width, settings.Height);
            var gridGenerator = new GridGenerator(gridBuilder, random);
            var hintGenerator = new HintGenerator(settings.PlayerCount, HintFunctions.SimpleModeFunctions);
            
            for (var i = 0; i < AttemptMaxCount; ++i)
            {
                
                var grid = gridGenerator.Generate();
                var allHints = hintGenerator.Generate(grid.ToImmutable());
                if (!allHints.Any())
                    continue;
                var (grail, hints) = allHints[random.Next(allHints.Length - 1)];

                return new Level
                {
                    Grid = grid.ToImmutable(),
                    Hints = hints,
                    Grail = grail,
                };
            }

            throw new Exception("Too many attempts");
        }
    }
}