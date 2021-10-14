﻿using Discoverer.Logic.GameContract;
using Discoverer.Logic.Generator;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Process
{
    public class GameBuilder
    {
        public Game Create(GameSettings settings)
        {
            var generator = new LevelGenerator();
            var level = generator.Generate(settings);
            
            IGridBuilder gridBuilder = 
                settings.GridType == EGridType.Hex 
                    ? new HexBuilder(settings.Width, settings.Height) 
                    : new IsometricBuilder(settings.Width, settings.Height);

            var processUpdater = new ProcessUpdater();
            var game = new Game(gridBuilder, processUpdater, HintFunctions.SimpleModeFunctions);
            game.Init(level, settings);

            return game;
        }
    }
}