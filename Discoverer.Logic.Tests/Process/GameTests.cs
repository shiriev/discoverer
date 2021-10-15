using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Process.Contracts;
using Discoverer.Logic.Tests.Mocks;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Process
{
    public class GameTests
    {
        public static IEnumerable<TestCaseData> SaveStateTestData
        {
            get
            {
                var players = new List<Player>
                {
                    new() { Order = 0, Name = "One", Id = Guid.NewGuid() },
                    new() { Order = 1, Name = "Two", Id = Guid.NewGuid() },
                };
                
                var level = new Level
                {
                    Grid = new TestGrid<Region>(5).ToImmutable(),
                    Hints = new [] { EHint.SwampOrWater, EHint.DesertOrSwamp },
                    Grail = new TestCoordinate { I = 0 },
                };
                
                yield return new TestCaseData(
                    new ProcessState
                    (
                        Actions: ImmutableList<GameAction>.Empty,
                        MarkerSetGrid: new TestGrid<MarkerSet>(5).ToImmutable(),
                        CurrentPlayerNum: 0,
                        CurrentTurn: 0,
                        GameState: new GameNotStartedState(),
                        GameId: Guid.NewGuid(),
                        PlayerCount: 2
                    ),
                    null
                );
            }
        }
        
        [TestCaseSource(nameof(SaveStateTestData))]
        public void SaveState_ForTestCases_ThrowsOrNot(object processStateObject, Type expectedExceptionType)
        {
            var processState = (ProcessState) processStateObject;
            //var process = new GameProcess();


        }
        
        [Test]
        public void RunCommand_AskQuestion_On()
        {
            
        }
    }   
}