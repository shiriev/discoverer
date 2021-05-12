using System;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Generator;
using Discoverer.Logic.Settings;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Generator
{
    public class LevelGeneratorTests
    {
        [Test]
        public void Generate_WithHex_Pass()
        {
            var generator = new LevelGenerator();

            var level = generator.Generate(new GenerationSettings
            {
                GridType = EGridType.Hex,
                Width = 10,
                Height = 10,
                PlayerCount = 3,
            });
            
            Assert.Pass();
        }
        
        [Test]
        public void Generate_WithIsometric_Pass()
        {
            var generator = new LevelGenerator();

            var level = generator.Generate(new GenerationSettings
            {
                GridType = EGridType.Isometric,
                Width = 10,
                Height = 10,
                PlayerCount = 3,
            });
            
            Assert.Pass();
        }
        
        [Test]
        public void Generate_With0Width_Throws()
        {
            var generator = new LevelGenerator();

            Assert.Throws<ArgumentException>( () => generator.Generate(new GenerationSettings
            {
                GridType = EGridType.Isometric,
                Width = 0,
                Height = 10,
                PlayerCount = 3,
            }));
        }
        
        [Test]
        public void Generate_With1Player_Throws()
        {
            var generator = new LevelGenerator();

            Assert.Throws<ArgumentOutOfRangeException>( () => generator.Generate(new GenerationSettings
            {
                GridType = EGridType.Isometric,
                Width = 10,
                Height = 10,
                PlayerCount = 1,
            }));
        }
        
        [Test]
        public void Generate_With0Player_Throws()
        {
            var generator = new LevelGenerator();

            Assert.Throws<ArgumentOutOfRangeException>( () => generator.Generate(new GenerationSettings
            {
                GridType = EGridType.Isometric,
                Width = 10,
                Height = 10,
                PlayerCount = 0,
            }));
        }
        
        [Test]
        public void Generate_WithMinus1Player_Throws()
        {
            var generator = new LevelGenerator();

            Assert.Throws<ArgumentOutOfRangeException>( () => generator.Generate(new GenerationSettings
            {
                GridType = EGridType.Isometric,
                Width = 10,
                Height = 10,
                PlayerCount = -1,
            }));
        }
    }
}