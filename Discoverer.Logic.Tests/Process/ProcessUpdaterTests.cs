using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
                var possibleCells = new TestGrid<bool[]>(new[]
                    {
                        new [] { false, true },
                        new [] { false, true },
                        new [] { true, true },
                        new [] { true, false },
                        new [] { true, false },
                    });
                
                var possibleCellsForFourPlayers = new TestGrid<bool[]>(new[]
                {
                    new [] { false, true,  false, false },
                    new [] { false, true,  true, false },
                    new [] { true,  true,  true,  true, },
                    new [] { true,  false, false, false },
                    new [] { true,  false, false, false },
                });
                
                var defaultProcessState = new ProcessState
                (
                    Actions: new List<GameAction>(),
                    CellStateGrid: new TestGrid<CellState>(5),
                    CurrentPlayerNum: 0,
                    CurrentTurn: 0,
                    GameState: new GameNotStartedState(),
                    GameId: Guid.NewGuid(),
                    PlayerCount: 2
                );
                
                var defaultProcessStateForFourPlayers = new ProcessState
                (
                    Actions: new List<GameAction>(),
                    CellStateGrid: new TestGrid<CellState>(5),
                    CurrentPlayerNum: 0,
                    CurrentTurn: 0,
                    GameState: new GameNotStartedState(),
                    GameId: Guid.NewGuid(),
                    PlayerCount: 4
                );
                
                for (var i = 0; i < defaultProcessState.CellStateGrid.Size; i++)
                {
                    defaultProcessState.CellStateGrid.Set(new TestCoordinate {I = i},
                        new CellState(null, ImmutableHashSet<int>.Empty));
                }
                
                for (var i = 0; i < defaultProcessStateForFourPlayers.CellStateGrid.Size; i++)
                {
                    defaultProcessStateForFourPlayers.CellStateGrid.Set(new TestCoordinate {I = i},
                        new CellState(null, ImmutableHashSet<int>.Empty));
                }
                
                var actions = new List<GameAction>
                {
                    new GameStartedAction()
                };
                
                yield return new TestCaseData(
                    defaultProcessState,
                    possibleCells,
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
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 1 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellOnStartAction(0, new TestCoordinate { I = 1 })
                            },
                            GameState = new PlayerPutsImproperCellOnStartState(),
                            CurrentPlayerNum = 1,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 1 }, new CellState(0, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellOnStartAction(0, new TestCoordinate { I = 1 })
                    }
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
                            CurrentPlayerNum = 1
                        }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                            },
                            GameState = new PlayerPutsImproperCellOnStartState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 1,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                    }
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
                            CurrentPlayerNum = 1,
                            CurrentTurn = 1
                        }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                            },
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 2,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                    }
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerMakesTurnState()
                        }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 0 }),
                                new PlayerAnsweredToQuestionAction(0, 1, true, new TestCoordinate { I = 0 })
                            },
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 1,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 0 }, new CellState(null, ImmutableHashSet<int>.Empty.Add(1)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 0 }),
                        new PlayerAnsweredToQuestionAction(0, 1, true, new TestCoordinate { I = 0 })
                    }
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerMakesTurnState()
                        }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 4 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 4 }),
                                new PlayerAnsweredToQuestionAction(0, 1, false, new TestCoordinate { I = 4 })
                            },
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 4 }),
                        new PlayerAnsweredToQuestionAction(0, 1, false, new TestCoordinate { I = 4 })
                    }
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerMakesTurnState()
                        }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerMadeGuessAction(0, new TestCoordinate { I = 4 }),
                                new PlayerAnsweredToGuessAction(0, 1, false, new TestCoordinate { I = 4 })
                            },
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(1, ImmutableHashSet<int>.Empty.Add(0)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerMadeGuessAction(0, new TestCoordinate { I = 4 }),
                        new PlayerAnsweredToGuessAction(0, 1, false, new TestCoordinate { I = 4 })
                    }
                );
                yield return new TestCaseData(
                    defaultProcessStateForFourPlayers.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 1,
                        }),
                    possibleCellsForFourPlayers,
                    new MakeGuessCommand(new TestCoordinate { I = 1 }),
                    defaultProcessStateForFourPlayers.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerMadeGuessAction(1, new TestCoordinate { I = 1 }),
                                new PlayerAnsweredToGuessAction(1, 2, true, new TestCoordinate { I = 1 }),
                                new PlayerAnsweredToGuessAction(1, 3, false, new TestCoordinate { I = 1 })
                            },
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CurrentPlayerNum = 1,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 1 }, new CellState(3, ImmutableHashSet<int>.Empty.Add(1).Add(2)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerMadeGuessAction(1, new TestCoordinate { I = 1 }),
                        new PlayerAnsweredToGuessAction(1, 2, true, new TestCoordinate { I = 1 }),
                        new PlayerAnsweredToGuessAction(1, 3, false, new TestCoordinate { I = 1 })
                    }
                );
                yield return new TestCaseData(
                    defaultProcessStateForFourPlayers.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 2,
                        }),
                    possibleCellsForFourPlayers,
                    new MakeGuessCommand(new TestCoordinate { I = 2 }),
                    defaultProcessStateForFourPlayers.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerMadeGuessAction(2, new TestCoordinate { I = 2 }),
                                new PlayerAnsweredToGuessAction(2, 3, true, new TestCoordinate { I = 2 }),
                                new PlayerAnsweredToGuessAction(2, 0, true, new TestCoordinate { I = 2 }),
                                new PlayerAnsweredToGuessAction(2, 1, true, new TestCoordinate { I = 2 }),
                                new PlayerWonAction(2)
                            },
                            GameState = new PlayerWinsGameState(),
                            CurrentPlayerNum = 2,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 2 }, new CellState(null, ImmutableHashSet<int>.Empty.Add(0).Add(1).Add(2).Add(3)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerMadeGuessAction(2, new TestCoordinate { I = 2 }),
                        new PlayerAnsweredToGuessAction(2, 3, true, new TestCoordinate { I = 2 }),
                        new PlayerAnsweredToGuessAction(2, 0, true, new TestCoordinate { I = 2 }),
                        new PlayerAnsweredToGuessAction(2, 1, true, new TestCoordinate { I = 2 }),
                        new PlayerWonAction(2)
                    }
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                        }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 1 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellAfterFailAction(0, new TestCoordinate { I = 1 })
                            },
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 1,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 1 }, new CellState(0, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellAfterFailAction(0, new TestCoordinate { I = 1 })
                    }
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CurrentPlayerNum = 1
                        }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                            },
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 1,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                    }
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            },
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CurrentPlayerNum = 1
                        }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Set(
                        new ProcessUpdate
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                            },
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 1,
                            CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                    }
                );
            }
        }
        
        [TestCaseSource(nameof(RunCommandTestData))]
        public void RunCommandTestData_ForTestCases_ReturnsCorrect(
            ProcessState processState, TestGrid<bool[]> possibleCells, GameCommand command, 
            ProcessState expectedResultState, List<GameAction> expectedActions)
        {
            var updater = new ProcessUpdater(new TestCoordinateHelper());

            var (resultProcess, actions) = updater.RunCommand(processState, possibleCells, command);

            CollectionAssert.AreEqual(expectedResultState.Actions, resultProcess.Actions);
            Assert.AreEqual(expectedResultState.CurrentPlayerNum, resultProcess.CurrentPlayerNum);
            Assert.AreEqual(expectedResultState.CurrentTurn, resultProcess.CurrentTurn);
            Assert.AreEqual(expectedResultState.GameState, resultProcess.GameState);
            Assert.AreEqual(expectedResultState.GameId, resultProcess.GameId);
            Assert.AreEqual(expectedResultState.PlayerCount, resultProcess.PlayerCount);
            CollectionAssert.AreEqual(expectedActions, actions);
            
            CollectionAssert.AreEqual(expectedResultState.CellStateGrid.Items, resultProcess.CellStateGrid.Items);
        }
        
        public static IEnumerable<TestCaseData> RunCommandErrorsTestData
        {
            get
            {
                var possibleCells = new TestGrid<bool[]>(new[]
                {
                    new [] { false, true },
                    new [] { false, true },
                    new [] { true, true },
                    new [] { true, false },
                    new [] { true, false },
                });
                
                var defaultProcessState = new ProcessState
                (
                    Actions: new List<GameAction>(),
                    CellStateGrid: new TestGrid<CellState>(5),
                    CurrentPlayerNum: 0,
                    CurrentTurn: 0,
                    GameState: new GameNotStartedState(),
                    GameId: Guid.NewGuid(),
                    PlayerCount: 2
                );
                
                yield return new TestCaseData(
                    defaultProcessState,
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState,
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState,
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState,
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerPutsImproperCellOnStartState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 0 }, new CellState(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new AskQuestionCommand(0, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerMakesTurnState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 0 }, new CellState(0, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerMakesTurnState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 0 }, new CellState(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerMakesTurnState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 0 }, new CellState(null, ImmutableHashSet<int>.Empty.Add(1)))
                    }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerMakesTurnState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(0, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerMakesTurnState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 4 }, new CellState(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerPutsImproperCellAfterFailState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 0 }, new CellState(0, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate
                    {
                        GameState = new PlayerPutsImproperCellAfterFailState(),
                        CellStateGrid = Helpers.CopyWithSet(defaultProcessState.CellStateGrid, new TestCoordinate { I = 0 }, new CellState(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Set(new ProcessUpdate { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
            }
        }
        
        [TestCaseSource(nameof(RunCommandErrorsTestData))]
        public void RunCommandTestData_ForTestCases_Throws(
            ProcessState processState, TestGrid<bool[]> possibleCells, GameCommand command, 
            Type expectedExceptionType)
        {
            var updater = new ProcessUpdater(new TestCoordinateHelper());

            Assert.Throws(expectedExceptionType, () => updater.RunCommand(processState, possibleCells, command));
        }
    }
}