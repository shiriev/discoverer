using System;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Generator
{
    public class LevelGenerator : ILevelGenerator
    {
        private const int AttemptMaxCount = 20;
        
        public Level Generate(GenerationSettings settings)
        {
            if (settings.PlayerCount <= 1)
                throw new ArgumentOutOfRangeException(nameof(settings.PlayerCount), settings.PlayerCount, null);
            
            var random = settings.Seed.HasValue ? new Random(settings.Seed.Value) : new Random();
            
            for (var i = 0; i < AttemptMaxCount; ++i)
            {
                IGrid<Cell> grid;
                ICoordinate grail;
                EHint[] hints;
                if (settings.GridType == EGridType.Hex)
                {
                    var gridBuilder = new HexBuilder(settings.Width, settings.Height);
                    var gridGenerator = new GridGenerator<HexCoordinate>(gridBuilder, random);
                    var hintGenerator = new HintGenerator<HexCoordinate>(settings.PlayerCount, HintFunctions<HexCoordinate>.SimpleModeFunctions);
                    var hexGrid = gridGenerator.Generate();
                    grid = hexGrid;
                    var allHints = hintGenerator.Generate(hexGrid);
                    if (!allHints.Any())
                        continue;
                    (grail, hints) = allHints[random.Next(allHints.Length - 1)];
                }
                else
                {
                    var gridBuilder = new IsometricBuilder(settings.Width, settings.Height);
                    var gridGenerator = new GridGenerator<IsometricCoordinate>(gridBuilder, random);
                    var hintGenerator = new HintGenerator<IsometricCoordinate>(settings.PlayerCount, HintFunctions<IsometricCoordinate>.SimpleModeFunctions);
                    var isometricGrid = gridGenerator.Generate();
                    grid = isometricGrid;
                    var allHints = hintGenerator.Generate(isometricGrid);
                    if (!allHints.Any())
                        continue;
                    (grail, hints) = allHints[random.Next(allHints.Length - 1)];
                }
                
                return new Level
                {
                    Grid = grid,
                    Hints = hints,
                    Grail = grail,
                };
            }

            throw new Exception("Too many attempts");
        }
    }
}