using System;
using System.Collections.Generic;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.GameContract.Actions;
using Discoverer.Logic.GameContract.Commands;
using Discoverer.Logic.GameContract.GameStates;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Process;
using Discoverer.Logic.Tests.Mocks;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Process
{
    public class ProcessUpdaterTests
    {
        public static IEnumerable<TestCaseData> RunCommandTestData
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
                var defaultProcessState = new ProcessState
                (
                    Actions: new List<GameAction>(),
                    CellStateGrid: new TestGrid<CellState>(),
                    Players: players,
                    CurrentPlayerId: players[0].Id,
                    CurrentTurn: 0,
                    GameState: new GameNotStartedState(),
                    GameId: Guid.NewGuid()
                );
                
                var actions = new List<GameAction>
                {
                    new GameStartedAction()
                };
                
                yield return new TestCaseData(
                    defaultProcessState,
                    new StartGameCommand(),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = actions,
                            GameState = new PlayerPutsImproperCellOnStartState(),
                        }),
                    actions
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerPutsImproperCellOnStartState(),
                        }),
                    level,
                    new PutImproperCellOnStartCommand(players[0], new TestCoordinate { I = 1 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new PlayerPutImproperCellOnStartAction(players[0], new TestCoordinate { I = 1 })
                            },
                            GameState = new PlayerPutsImproperCellOnStartState(),
                            CurrentPlayerId = players[1].Id,
                            CurrentTurn = 1,
                            
                        }),
                    actions
                );
            }
        }
        
        [TestCaseSource(nameof(RunCommandTestData))]
        public void RunCommandTestData_ForTestCases_ReturnsCorrect(
            ProcessState processState, Level level, GameCommand command, 
            ProcessState expectedResultState, List<GameAction> expectedActions)
        {
            var updater = new ProcessUpdater();

            var (resultProcess, actions) = updater.RunCommand(processState, level, command);

            CollectionAssert.AreEqual(expectedResultState.Actions, resultProcess.Actions);
            CollectionAssert.AreEqual(expectedResultState.Players, resultProcess.Players);
            Assert.AreEqual(expectedResultState.CurrentPlayerId, resultProcess.CurrentPlayerId);
            Assert.AreEqual(expectedResultState.CurrentTurn, resultProcess.CurrentTurn);
            Assert.AreEqual(expectedResultState.GameState, resultProcess.GameState);
            Assert.AreEqual(expectedResultState.GameId, resultProcess.GameId);
            CollectionAssert.AreEqual(expectedActions, actions);
            
            Assert.AreEqual(expectedResultState.CellStateGrid, resultProcess.CellStateGrid);
        }
    }
}