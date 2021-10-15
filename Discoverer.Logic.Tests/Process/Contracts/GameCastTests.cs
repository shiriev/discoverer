using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Actions;
using Discoverer.Logic.Contracts.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Process.Contracts;
using Discoverer.Logic.Tests.Mocks;
using NUnit.Framework;

namespace Discoverer.Logic.Tests.Process.Contracts
{
    public class GameCastTests
    {
        public static IEnumerable<TestCaseData> SetTestData
        {
            get
            {
                var guid = Guid.NewGuid();
                var firstProcessState = new GameCast(
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
                    new UpdateGameCastRequest(),
                    firstProcessState
                );
                
                yield return new TestCaseData(
                    firstProcessState,
                    new UpdateGameCastRequest()
                    {
                        Actions = ImmutableList<GameAction>.Empty.Add(new GameStartedAction()),
                        CurrentPlayerNum = 1
                    },
                    new GameCast(
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
                    new UpdateGameCastRequest()
                    {
                        MarkerSetGrid = new TestGrid<MarkerSet>(5)
                            .ToImmutable()
                            .CopyWithSet(new TestCoordinate { I = 0 }, new MarkerSet(null, ImmutableHashSet<int>.Empty)),
                        CurrentTurn = 1,
                        GameState = new PlayerMakesTurnState()
                    },
                    new GameCast(
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
        public void Set_ForTestCases_ReturnCorrectObject(object gameCastObject, object updateGameCastRequestObject, object expectedObject)
        {
            var gameCast = (GameCast) gameCastObject;
            var updateGameCastRequest = (UpdateGameCastRequest) updateGameCastRequestObject;
            var expected = (GameCast) expectedObject;
            
            var actual = gameCast.Update(updateGameCastRequest);

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