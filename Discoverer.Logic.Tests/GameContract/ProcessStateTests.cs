using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.GameContract.Actions;
using Discoverer.Logic.GameContract.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Tests.Mocks;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.GameContract
{
    public class ProcessStateTests
    {
        public static IEnumerable<TestCaseData> SetTestData
        {
            get
            {
                var guid = Guid.NewGuid();
                var firstProcessState = new ProcessState(
                    Actions: ImmutableList<GameAction>.Empty,
                    MarkerSetGrid: new TestGrid<MarkerSet>(5).ToImmutable(),
                    CurrentPlayerNum: 0,
                    CurrentTurn: 0,
                    GameState: new GameNotStartedState(),
                    PlayerCount: 2,
                    GameId: guid
                );
                
                yield return new TestCaseData(
                    firstProcessState,
                    new ProcessUpdate(),
                    firstProcessState
                );
                
                yield return new TestCaseData(
                    firstProcessState,
                    new ProcessUpdate()
                    {
                        Actions = ImmutableList<GameAction>.Empty.Add(new GameStartedAction()),
                        CurrentPlayerNum = 1
                    },
                    new ProcessState(
                        Actions: ImmutableList<GameAction>.Empty.Add(new GameStartedAction()),
                        MarkerSetGrid: new TestGrid<MarkerSet>(5).ToImmutable(),
                        CurrentPlayerNum: 1,
                        CurrentTurn: 0,
                        GameState: new GameNotStartedState(),
                        PlayerCount: 2,
                        GameId: guid
                    )
                );
                
                yield return new TestCaseData(
                    firstProcessState,
                    new ProcessUpdate()
                    {
                        MarkerSetGrid = new TestGrid<MarkerSet>(5)
                            .ToImmutable()
                            .CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(null, ImmutableHashSet<int>.Empty)),
                        CurrentTurn = 1,
                        GameState = new PlayerMakesTurnState()
                    },
                    new ProcessState(
                        Actions: ImmutableList<GameAction>.Empty,
                        MarkerSetGrid: new TestGrid<MarkerSet>(5)
                            .ToImmutable()
                            .CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(null, ImmutableHashSet<int>.Empty)),
                        CurrentPlayerNum: 0,
                        CurrentTurn: 1,
                        GameState: new PlayerMakesTurnState(),
                        PlayerCount: 2,
                        GameId: guid
                    )
                );
            }
        }

        [TestCaseSource(nameof(SetTestData))]
        public void Set_ForTestCases_ReturnCorrectObject(ProcessState processState, ProcessUpdate processUpdate, ProcessState expected)
        {
            var actual = processState.Set(processUpdate);

            Assert.AreEqual(expected.Actions, actual.Actions);
            CollectionAssert.AreEqual(expected.MarkerSetGrid.Items, actual.MarkerSetGrid.Items);
            Assert.AreEqual(expected.CurrentPlayerNum, actual.CurrentPlayerNum);
            Assert.AreEqual(expected.CurrentTurn, actual.CurrentTurn);
            Assert.AreEqual(expected.GameState, actual.GameState);
            Assert.AreEqual(expected.GameId, actual.GameId);
            Assert.AreEqual(expected.PlayerCount, actual.PlayerCount);
        }
    }
}