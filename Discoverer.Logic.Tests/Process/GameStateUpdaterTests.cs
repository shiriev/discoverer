﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Actions;
using Discoverer.Logic.Contracts.Commands;
using Discoverer.Logic.Contracts.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Process;
using Discoverer.Logic.Process.Contracts;
using Discoverer.Logic.Tests.Mocks;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Process
{
    public class GameStateUpdaterTests
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

                var defaultMarkerSetGrid = new TestGrid<MarkerSet>(5);
                
                for (var i = 0; i < defaultMarkerSetGrid.Size; i++)
                {
                    defaultMarkerSetGrid.Set(new TestCoordinate {I = i},
                        new MarkerSet(null, ImmutableHashSet<int>.Empty));
                }
                    
                var defaultProcessState = new GameCast
                (
                    Actions: ImmutableList<GameAction>.Empty,
                    MarkerSetGrid: defaultMarkerSetGrid.ToImmutable(),
                    CurrentPlayerNum: 0,
                    CurrentTurn: 0,
                    GameState: new GameNotStartedState(),
                    GameId: Guid.NewGuid(),
                    PlayerCount: 2
                );
                
                var defaultMarkerSetGridForFourPlayers = new TestGrid<MarkerSet>(5);

                for (var i = 0; i < defaultMarkerSetGridForFourPlayers.Size; i++)
                {
                    defaultMarkerSetGridForFourPlayers.Set(new TestCoordinate {I = i},
                        new MarkerSet(null, ImmutableHashSet<int>.Empty));
                }
                
                var defaultProcessStateForFourPlayers = new GameCast
                (
                    Actions: ImmutableList<GameAction>.Empty,
                    MarkerSetGrid: defaultMarkerSetGridForFourPlayers.ToImmutable(),
                    CurrentPlayerNum: 0,
                    CurrentTurn: 0,
                    GameState: new GameNotStartedState(),
                    GameId: Guid.NewGuid(),
                    PlayerCount: 4
                );
                
                var actions = new List<GameAction>
                {
                    new GameStartedAction()
                }.ToImmutableList();
                
                yield return new TestCaseData(
                    defaultProcessState,
                    possibleCells,
                    new StartGameCommand(),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = actions,
                            GameState = new PlayerPutsImproperCellOnStartState(),
                        }),
                    actions
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellOnStartState(),
                        }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 1 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellOnStartAction(0, new TestCoordinate { I = 1 })
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellOnStartState(),
                            CurrentPlayerNum = 1,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 1 }, new MarkerSet(0, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellOnStartAction(0, new TestCoordinate { I = 1 })
                    }.ToImmutableList()
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellOnStartState(),
                            CurrentPlayerNum = 1
                        }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellOnStartState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 1,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                    }.ToImmutableList()
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellOnStartState(),
                            CurrentPlayerNum = 1,
                            CurrentTurn = 1
                        }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 2,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellOnStartAction(1, new TestCoordinate { I = 4 })
                    }.ToImmutableList()
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState()
                        }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 0 }),
                                new PlayerAnsweredToQuestionAction(0, 1, true, new TestCoordinate { I = 0 })
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 1,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(null, ImmutableHashSet<int>.Empty.Add(1)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 0 }),
                        new PlayerAnsweredToQuestionAction(0, 1, true, new TestCoordinate { I = 0 })
                    }.ToImmutableList()
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState()
                        }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 4 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 4 }),
                                new PlayerAnsweredToQuestionAction(0, 1, false, new TestCoordinate { I = 4 })
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerAskedQuestionAction(0, 1, new TestCoordinate { I = 4 }),
                        new PlayerAnsweredToQuestionAction(0, 1, false, new TestCoordinate { I = 4 })
                    }.ToImmutableList()
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState()
                        }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerMadeGuessAction(0, new TestCoordinate { I = 4 }),
                                new PlayerAnsweredToGuessAction(0, 1, false, new TestCoordinate { I = 4 })
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(1, ImmutableHashSet<int>.Empty.Add(0)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerMadeGuessAction(0, new TestCoordinate { I = 4 }),
                        new PlayerAnsweredToGuessAction(0, 1, false, new TestCoordinate { I = 4 })
                    }.ToImmutableList()
                );
                yield return new TestCaseData(
                    defaultProcessStateForFourPlayers.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 1,
                        }),
                    possibleCellsForFourPlayers,
                    new MakeGuessCommand(new TestCoordinate { I = 1 }),
                    defaultProcessStateForFourPlayers.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerMadeGuessAction(1, new TestCoordinate { I = 1 }),
                                new PlayerAnsweredToGuessAction(1, 2, true, new TestCoordinate { I = 1 }),
                                new PlayerAnsweredToGuessAction(1, 3, false, new TestCoordinate { I = 1 })
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CurrentPlayerNum = 1,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 1 }, new MarkerSet(3, ImmutableHashSet<int>.Empty.Add(1).Add(2)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerMadeGuessAction(1, new TestCoordinate { I = 1 }),
                        new PlayerAnsweredToGuessAction(1, 2, true, new TestCoordinate { I = 1 }),
                        new PlayerAnsweredToGuessAction(1, 3, false, new TestCoordinate { I = 1 })
                    }.ToImmutableList()
                );
                yield return new TestCaseData(
                    defaultProcessStateForFourPlayers.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 2,
                        }),
                    possibleCellsForFourPlayers,
                    new MakeGuessCommand(new TestCoordinate { I = 2 }),
                    defaultProcessStateForFourPlayers.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerMadeGuessAction(2, new TestCoordinate { I = 2 }),
                                new PlayerAnsweredToGuessAction(2, 3, true, new TestCoordinate { I = 2 }),
                                new PlayerAnsweredToGuessAction(2, 0, true, new TestCoordinate { I = 2 }),
                                new PlayerAnsweredToGuessAction(2, 1, true, new TestCoordinate { I = 2 }),
                                new PlayerWonAction(2)
                            }.ToImmutableList(),
                            GameState = new PlayerWinsGameState(),
                            CurrentPlayerNum = 2,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid
                                .CopyWithSet(
                                    new TestCoordinate { I = 2 }, 
                                    new MarkerSet(null, ImmutableHashSet<int>.Empty
                                        .Add(0).Add(1).Add(2).Add(3)))
                        }),
                    new List<GameAction>
                    {
                        new PlayerMadeGuessAction(2, new TestCoordinate { I = 2 }),
                        new PlayerAnsweredToGuessAction(2, 3, true, new TestCoordinate { I = 2 }),
                        new PlayerAnsweredToGuessAction(2, 0, true, new TestCoordinate { I = 2 }),
                        new PlayerAnsweredToGuessAction(2, 1, true, new TestCoordinate { I = 2 }),
                        new PlayerWonAction(2)
                    }.ToImmutableList()
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                        }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 1 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellAfterFailAction(0, new TestCoordinate { I = 1 })
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 1,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 1 }, new MarkerSet(0, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellAfterFailAction(0, new TestCoordinate { I = 1 })
                    }.ToImmutableList()
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CurrentPlayerNum = 1
                        }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 1,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                    }.ToImmutableList()
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction()
                            }.ToImmutableList(),
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                            CurrentPlayerNum = 1
                        }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 4 }),
                    defaultProcessState.Update(
                        new UpdateGameCastRequest
                        {
                            Actions = new List<GameAction>
                            {
                                new GameStartedAction(),
                                new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                            }.ToImmutableList(),
                            GameState = new PlayerMakesTurnState(),
                            CurrentPlayerNum = 0,
                            CurrentTurn = 1,
                            MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                        }),
                    new List<GameAction>
                    {
                        new PlayerPutImproperCellAfterFailAction(1, new TestCoordinate { I = 4 })
                    }.ToImmutableList()
                );
            }
        }
        
        [TestCaseSource(nameof(RunCommandTestData))]
        public void RunCommandTestData_ForTestCases_ReturnsCorrect(
            object gameCastObject, TestGrid<bool[]> possibleCells, GameCommand command, 
            object expectedResultStateObject, ImmutableList<GameAction> expectedActions)
        {
            var gameCast = (GameCast) gameCastObject;
            var expectedResultState = (GameCast) expectedResultStateObject;
            var updater = new GameStateUpdater();

            var (resultProcess, actions) = updater.RunCommand(gameCast, possibleCells, command);

            CollectionAssert.AreEqual(expectedResultState.Actions, resultProcess.Actions);
            Assert.AreEqual(expectedResultState.CurrentPlayerNum, resultProcess.CurrentPlayerNum);
            Assert.AreEqual(expectedResultState.CurrentTurn, resultProcess.CurrentTurn);
            Assert.AreEqual(expectedResultState.GameState, resultProcess.GameState);
            Assert.AreEqual(expectedResultState.GameId, resultProcess.GameId);
            Assert.AreEqual(expectedResultState.PlayerCount, resultProcess.PlayerCount);
            CollectionAssert.AreEqual(expectedActions, actions);
            
            CollectionAssert.AreEqual(expectedResultState.MarkerSetGrid.Items, resultProcess.MarkerSetGrid.Items);
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
                
                var defaultProcessState = new GameCast
                (
                    Actions: ImmutableList<GameAction>.Empty,
                    MarkerSetGrid: new TestGrid<MarkerSet>(5).ToImmutable(),
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
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 1 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellOnStartState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerPutsImproperCellOnStartState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new AskQuestionCommand(0, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerMakesTurnState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(0, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerMakesTurnState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerMakesTurnState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(null, ImmutableHashSet<int>.Empty.Add(1)))
                    }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerMakesTurnState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(0, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerMakesTurnState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 4 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerMakesTurnState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerPutsImproperCellAfterFailState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 4 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerPutsImproperCellAfterFailState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(0, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest
                    {
                        GameState = new PlayerPutsImproperCellAfterFailState(),
                        MarkerSetGrid = defaultProcessState.MarkerSetGrid.CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(1, ImmutableHashSet<int>.Empty))
                    }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new StartGameCommand(),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new PutImproperCellOnStartCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new PutImproperCellAfterFailCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new AskQuestionCommand(1, new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
                yield return new TestCaseData(
                    defaultProcessState.Update(new UpdateGameCastRequest { GameState = new PlayerWinsGameState() }),
                    possibleCells,
                    new MakeGuessCommand(new TestCoordinate { I = 0 }),
                    typeof(ArgumentException)
                );
            }
        }
        
        [TestCaseSource(nameof(RunCommandErrorsTestData))]
        public void RunCommandTestData_ForTestCases_Throws(
            object gameCastObject, TestGrid<bool[]> possibleCells, GameCommand command, 
            Type expectedExceptionType)
        {
            var gameCast = (GameCast) gameCastObject;
            var updater = new GameStateUpdater();

            Assert.Throws(expectedExceptionType, () => updater.RunCommand(gameCast, possibleCells, command));
        }
    }
}