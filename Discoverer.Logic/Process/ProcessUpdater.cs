using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Actions;
using Discoverer.Logic.Contracts.Commands;
using Discoverer.Logic.Contracts.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Process.Contracts;

namespace Discoverer.Logic.Process
{
    internal sealed class ProcessUpdater : IProcessUpdater
    {
        public (GameCast, ImmutableList<GameAction>) RunCommand(GameCast gameCast, IGrid<bool[]> possibleCells, GameCommand command)
        {
            if (command is StartGameCommand startGameCommand && gameCast.GameState is GameNotStartedState)
            {
                return RunStartGameCommandOnGameNotStartedState(gameCast, possibleCells, startGameCommand);
            } 
            if (command is PutImproperCellOnStartCommand putImproperCellOnStartCommand && gameCast.GameState is PlayerPutsImproperCellOnStartState)
            {
                return RunPutImproperCellOnStartCommandOnPlayerPutsImproperCellOnStartState(gameCast, possibleCells, putImproperCellOnStartCommand);
            } 
            if (command is AskQuestionCommand askQuestionCommand && gameCast.GameState is PlayerMakesTurnState)
            {
                return RunAskQuestionCommandOnPlayerMakesTurnState(gameCast, possibleCells, askQuestionCommand);
            }
            if (command is MakeGuessCommand makeGuessCommand && gameCast.GameState is PlayerMakesTurnState)
            {
                 return RunMakeGuessCommandOnPlayerMakesTurnState(gameCast, possibleCells, makeGuessCommand);
            }
            if (command is PutImproperCellAfterFailCommand putImproperCellAfterFailCommand && gameCast.GameState is PlayerPutsImproperCellAfterFailState)
            {
                return RunPutImproperCellAfterFailCommandOnPlayerPutsImproperCellAfterFailState(gameCast, possibleCells, putImproperCellAfterFailCommand);
            } 
            throw new ArgumentException($"Player cannot run {command.GetType().Name} when game on {gameCast.GameState.GetType().Name} state", nameof(command));
        }

        // TODO: Implement without try catch
        public bool GetCommandPossibility(GameCast gameCast, IGrid<bool[]> possibleCells, GameCommand command)
        {
            try
            {
                RunCommand(gameCast, possibleCells, command);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static (GameCast, ImmutableList<GameAction>) RunStartGameCommandOnGameNotStartedState(GameCast gameCast, IGrid<bool[]> possibleCells, StartGameCommand command)
        {
            var actions = ImmutableList<GameAction>.Empty.Add(new GameStartedAction());
            return (
                gameCast.Update(new UpdateGameCastRequest
                {
                    Actions = actions,
                    GameState = new PlayerPutsImproperCellOnStartState(),
                }),
                actions);
        }
        
        private static (GameCast, ImmutableList<GameAction>) RunPutImproperCellOnStartCommandOnPlayerPutsImproperCellOnStartState(GameCast gameCast, IGrid<bool[]> possibleCells, PutImproperCellOnStartCommand command)
        {
            if (possibleCells.Get(command.Coordinate)[gameCast.CurrentPlayerNum] == true)
            {
                throw new ArgumentException($"Player cannot put just one improper cell on possible cell ({command.Coordinate})", nameof(command));
            }
            
            if (gameCast.MarkerSetGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }

            var actions = ImmutableList<GameAction>.Empty.Add(new PlayerPutImproperCellOnStartAction(gameCast.CurrentPlayerNum, command.Coordinate));
            
            var newTurn = gameCast.CurrentPlayerNum == gameCast.PlayerCount - 1 ? gameCast.CurrentTurn + 1 : gameCast.CurrentTurn;
            var newOrder = gameCast.CurrentPlayerNum == gameCast.PlayerCount - 1
                ? 0
                : gameCast.CurrentPlayerNum + 1;

            var newState = newTurn == 2 ? (GameState)new PlayerMakesTurnState() : new PlayerPutsImproperCellOnStartState();

            var oldMarkerSet = gameCast.MarkerSetGrid.Get(command.Coordinate);
            var newMarkerSetGrid = gameCast.MarkerSetGrid.CopyWithSet(command.Coordinate, 
                new MarkerSet(
                    gameCast.CurrentPlayerNum, 
                    oldMarkerSet.PossibleCellForPlayers
                )
            );

            return (
                gameCast.Update(new UpdateGameCastRequest
                {
                    Actions = gameCast.Actions.Concat(actions).ToImmutableList(),
                    MarkerSetGrid = newMarkerSetGrid,
                    CurrentPlayerNum = newOrder,
                    CurrentTurn = newTurn,
                    GameState = newState,
                }),
                actions);
        }
        
        private static (GameCast, ImmutableList<GameAction>) RunAskQuestionCommandOnPlayerMakesTurnState(GameCast gameCast, IGrid<bool[]> possibleCells, AskQuestionCommand command)
        {
            if (gameCast.CurrentPlayerNum == command.AnsweringPlayerNum)
            {
                throw new ArgumentException($"Player cannot ask himself (player num - {command.AnsweringPlayerNum})", nameof(command));
            }
            
            if (gameCast.MarkerSetGrid.Get(command.Coordinate).PossibleCellForPlayers.Contains(command.AnsweringPlayerNum))
            {
                throw new ArgumentException($"Player cannot ask if answering player already has answer on coordinate ({command.Coordinate})", nameof(command));
            }
            
            if (gameCast.MarkerSetGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }

            var answer = possibleCells.Get(command.Coordinate)[command.AnsweringPlayerNum];

            var actions = ImmutableList<GameAction>.Empty
                .Add(new PlayerAskedQuestionAction(gameCast.CurrentPlayerNum, command.AnsweringPlayerNum, command.Coordinate))
                .Add(new PlayerAnsweredToQuestionAction(gameCast.CurrentPlayerNum, command.AnsweringPlayerNum, answer, command.Coordinate));

            if (answer == true)
            {
                var newTurn = gameCast.CurrentPlayerNum == gameCast.PlayerCount - 1 ? gameCast.CurrentTurn + 1 : gameCast.CurrentTurn;
                var newOrder = gameCast.CurrentPlayerNum == gameCast.PlayerCount - 1
                    ? 0
                    : gameCast.CurrentPlayerNum + 1;

                var oldMarkerSet = gameCast.MarkerSetGrid.Get(command.Coordinate);
                var newMarkerSetGrid = gameCast.MarkerSetGrid.CopyWithSet(command.Coordinate, 
                    new MarkerSet(
                        oldMarkerSet.ImproperCellForPlayer,
                        oldMarkerSet.PossibleCellForPlayers.Add(command.AnsweringPlayerNum)
                    ));

                return (
                    gameCast.Update(new UpdateGameCastRequest
                    {
                        Actions = gameCast.Actions.Concat(actions).ToImmutableList(),
                        MarkerSetGrid = newMarkerSetGrid,
                        CurrentPlayerNum = newOrder,
                        CurrentTurn = newTurn,
                        GameState = new PlayerMakesTurnState(),
                    }),
                    actions);
            }
            else
            {
                var oldMarkerSet = gameCast.MarkerSetGrid.Get(command.Coordinate);
                var newMarkerSetGrid = gameCast.MarkerSetGrid.CopyWithSet(command.Coordinate, 
                    new MarkerSet(
                        command.AnsweringPlayerNum,
                        oldMarkerSet.PossibleCellForPlayers
                    ));

                return (
                    gameCast.Update(new UpdateGameCastRequest
                    {
                        Actions = gameCast.Actions.Concat(actions).ToImmutableList(),
                        MarkerSetGrid = newMarkerSetGrid,
                        GameState = new PlayerPutsImproperCellAfterFailState(),
                    }),
                    actions);
            }
        }
        
        private static (GameCast, ImmutableList<GameAction>) RunMakeGuessCommandOnPlayerMakesTurnState(GameCast gameCast, IGrid<bool[]> possibleCells, MakeGuessCommand command)
        {
            if (possibleCells.Get(command.Coordinate)[gameCast.CurrentPlayerNum] == false)
            {
                throw new ArgumentException($"Player cannot put just one improper cell on impossible cell ({command.Coordinate})", nameof(command));
            }
            
            if (gameCast.MarkerSetGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }

            var actions = new List<GameAction>
            {
                new PlayerMadeGuessAction(gameCast.CurrentPlayerNum, command.Coordinate)
            };
            
            var oldMarkerSet = gameCast.MarkerSetGrid.Get(command.Coordinate);
            var possibles = oldMarkerSet.PossibleCellForPlayers;
            possibles = possibles.Add(gameCast.CurrentPlayerNum);

            var newMarkerSetGrid = gameCast.MarkerSetGrid.CopyGrid();

            for (var num = (gameCast.CurrentPlayerNum + 1) % gameCast.PlayerCount; num != gameCast.CurrentPlayerNum; num = (num + 1) % gameCast.PlayerCount)
            {
                var answer = possibleCells.Get(command.Coordinate)[num];
                actions.Add(new PlayerAnsweredToGuessAction(gameCast.CurrentPlayerNum, num, answer, command.Coordinate));
                if (answer == true)
                {
                    possibles = possibles.Add(num);
                }
                else
                {
                    newMarkerSetGrid.Set(command.Coordinate, 
                        new MarkerSet(
                            num,
                            possibles
                        )
                    );
                    return (
                        gameCast.Update(new UpdateGameCastRequest
                        {
                            Actions = gameCast.Actions.Concat(actions).ToImmutableList(),
                            MarkerSetGrid = newMarkerSetGrid.ToImmutable(),
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                        }),
                        actions.ToImmutableList());
                }
            }
            
            actions.Add(new PlayerWonAction(gameCast.CurrentPlayerNum));

            newMarkerSetGrid.Set(command.Coordinate, 
                new MarkerSet(
                    oldMarkerSet.ImproperCellForPlayer,
                    possibles
                )
            );

            return (
                gameCast.Update(new UpdateGameCastRequest
                {
                    Actions = gameCast.Actions.Concat(actions).ToImmutableList(),
                    MarkerSetGrid = newMarkerSetGrid.ToImmutable(),
                    GameState = new PlayerWinsGameState(),
                }),
                actions.ToImmutableList());
        }
    
        private static (GameCast, ImmutableList<GameAction>) RunPutImproperCellAfterFailCommandOnPlayerPutsImproperCellAfterFailState(GameCast gameCast, IGrid<bool[]> possibleCells, PutImproperCellAfterFailCommand command)
        {
            if (possibleCells.Get(command.Coordinate)[gameCast.CurrentPlayerNum] == true)
            {
                throw new ArgumentException($"Player cannot put just one improper cell on possible cell ({command.Coordinate})", nameof(command));
            }
            
            if (gameCast.MarkerSetGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }

            var actions = new List<GameAction>
            {
                new PlayerPutImproperCellAfterFailAction(gameCast.CurrentPlayerNum, command.Coordinate)
            };
            
            var newTurn = gameCast.CurrentPlayerNum == gameCast.PlayerCount - 1 ? gameCast.CurrentTurn + 1 : gameCast.CurrentTurn;
            var newOrder = gameCast.CurrentPlayerNum == gameCast.PlayerCount - 1
                ? 0
                : gameCast.CurrentPlayerNum + 1;

            var oldMarkerSet = gameCast.MarkerSetGrid.Get(command.Coordinate);
            var newMarkerSetGrid = gameCast.MarkerSetGrid.CopyWithSet(command.Coordinate, 
                new MarkerSet(
                    gameCast.CurrentPlayerNum, 
                    oldMarkerSet.PossibleCellForPlayers
                )
            );

            return (
                gameCast.Update(new UpdateGameCastRequest
                {
                    Actions = gameCast.Actions.Concat(actions).ToImmutableList(),
                    MarkerSetGrid = newMarkerSetGrid,
                    CurrentPlayerNum = newOrder,
                    CurrentTurn = newTurn,
                    GameState = new PlayerMakesTurnState(),
                }),
                actions.ToImmutableList());
        }
    }
}