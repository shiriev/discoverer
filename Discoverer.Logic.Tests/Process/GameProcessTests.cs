using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.GameContract.GameStates;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Tests.Mocks;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Process
{
    public class GameProcessTests
    {
        public static IEnumerable<TestCaseData> SaveStateTestData
        {
            get
            {
                var players = new List<Player>
                {
                    new (0, "One", Guid.NewGuid()),
                    new (1, "Two", Guid.NewGuid()),
                };
                
                var level = new Level
                {
                    Grid = new TestGrid<Cell>(),
                    Hints = new [] { EHint.SwampOrWater, EHint.DesertOrSwamp },
                    Grail = new TestCoordinate { I = 0 },
                };
                
                yield return new TestCaseData(
                    new ProcessState
                    (
                        Actions: new List<GameAction>(),
                        CellStateGrid: new TestGrid<CellState>(),
                        Players: players,
                        CurrentPlayerId: players.First().Id,
                        CurrentTurn: 0,
                        GameState: new GameNotStartedState(),
                        GameId: Guid.NewGuid()
                    ),
                    null
                );
            }
        }
        
        [TestCaseSource(nameof(SaveStateTestData))]
        public void SaveState_ForTestCases_ThrowsOrNot(ProcessState processState, Type expectedExceptionType)
        {
            //var process = new GameProcess();
            
            
        }
        
        [Test]
        public void RunCommand_AskQuestion_On()
        {
            
        }
    }   
}