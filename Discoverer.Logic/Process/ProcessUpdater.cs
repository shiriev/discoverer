﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.GameContract.Actions;
using Discoverer.Logic.GameContract.Commands;
using Discoverer.Logic.GameContract.GameStates;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Process
{
    public class ProcessUpdater : IProcessUpdater
    {
        private readonly ICoordinateHelper _coordinateHelper;

        public ProcessUpdater(ICoordinateHelper coordinateHelper)
        {
            _coordinateHelper = coordinateHelper ?? throw new ArgumentNullException(nameof(coordinateHelper));
        }
        
        public (ProcessState, List<GameAction>) RunCommand(ProcessState processState, IGrid<bool[]> possibleCells, GameCommand command)
        {
            if (command is StartGameCommand startGameCommand && processState.GameState is GameNotStartedState)
            {
                return RunStartGameCommandOnGameNotStartedState(processState, possibleCells, startGameCommand);
            } 
            if (command is PutImproperCellOnStartCommand putImproperCellOnStartCommand && processState.GameState is PlayerPutsImproperCellOnStartState)
            {
                return RunPutImproperCellOnStartCommandOnPlayerPutsImproperCellOnStartState(processState, possibleCells, putImproperCellOnStartCommand);
            } 
            if (command is AskQuestionCommand askQuestionCommand && processState.GameState is PlayerMakesTurnState)
            {
                return RunAskQuestionCommandOnPlayerMakesTurnState(processState, possibleCells, askQuestionCommand);
            }
            if (command is MakeGuessCommand makeGuessCommand && processState.GameState is PlayerMakesTurnState)
            {
                 return RunMakeGuessCommandOnPlayerMakesTurnState(processState, possibleCells, makeGuessCommand);
            }
            if (command is PutImproperCellAfterFailCommand putImproperCellAfterFailCommand && processState.GameState is PlayerPutsImproperCellAfterFailState)
            {
                return RunPutImproperCellAfterFailCommandOnPlayerPutsImproperCellAfterFailState(processState, possibleCells, putImproperCellAfterFailCommand);
            } 
            throw new ArgumentException($"Player cannot run {command.GetType().Name} when game on {processState.GameState.GetType().Name} state", nameof(command));
        }

        // TODO: Implement without try catch
        public bool GetCommandPossibility(ProcessState processState, IGrid<bool[]> possibleCells, GameCommand command)
        {
            try
            {
                RunCommand(processState, possibleCells, command);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private (ProcessState, List<GameAction>) RunStartGameCommandOnGameNotStartedState(ProcessState processState, IGrid<bool[]> possibleCells, StartGameCommand command)
        {
            var actions = new List<GameAction>
            {
                new GameStartedAction()
            };
            return (
                processState.Set(new ProcessUpdate
                {
                    Actions = actions,
                    GameState = new PlayerPutsImproperCellOnStartState(),
                }),
                actions);
        }
        
        private (ProcessState, List<GameAction>) RunPutImproperCellOnStartCommandOnPlayerPutsImproperCellOnStartState(ProcessState processState, IGrid<bool[]> possibleCells, PutImproperCellOnStartCommand command)
        {
            if (possibleCells.Get(command.Coordinate)[processState.CurrentPlayerNum] == true)
            {
                throw new ArgumentException($"Player cannot put just one improper cell on possible cell ({command.Coordinate})", nameof(command));
            }
            
            if (processState.CellStateGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }

            var actions = new List<GameAction>
            {
                new PlayerPutImproperCellOnStartAction(processState.CurrentPlayerNum, command.Coordinate)
            };
            
            var newTurn = processState.CurrentPlayerNum == processState.PlayerCount - 1 ? processState.CurrentTurn + 1 : processState.CurrentTurn;
            var newOrder = processState.CurrentPlayerNum == processState.PlayerCount - 1
                ? 0
                : processState.CurrentPlayerNum + 1;

            var newState = newTurn == 2 ? (GameState)new PlayerMakesTurnState() : new PlayerPutsImproperCellOnStartState();

            var newGrid = processState.CellStateGrid.Copy();
            var oldCell = processState.CellStateGrid.Get(command.Coordinate);
            newGrid.Set(command.Coordinate, 
                new CellState(
                    processState.CurrentPlayerNum, 
                    oldCell.PossibleCellForPlayers
                )
            );

            return (
                processState.Set(new ProcessUpdate
                {
                    Actions = processState.Actions.Concat(actions).ToList(),
                    CellStateGrid = newGrid,
                    CurrentPlayerNum = newOrder,
                    CurrentTurn = newTurn,
                    GameState = newState,
                }),
                actions);
        }
        
        private (ProcessState, List<GameAction>) RunAskQuestionCommandOnPlayerMakesTurnState(ProcessState processState, IGrid<bool[]> possibleCells, AskQuestionCommand command)
        {
            if (processState.CurrentPlayerNum == command.AnsweringPlayerNum)
            {
                throw new ArgumentException($"Player cannot ask himself (player num - {command.AnsweringPlayerNum})", nameof(command));
            }
            
            if (processState.CellStateGrid.Get(command.Coordinate).PossibleCellForPlayers.Contains(command.AnsweringPlayerNum))
            {
                throw new ArgumentException($"Player cannot ask if answering player already has answer on coordinate ({command.Coordinate})", nameof(command));
            }
            
            if (processState.CellStateGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }

            var answer = possibleCells.Get(command.Coordinate)[command.AnsweringPlayerNum];

            var actions = new List<GameAction>
            {
                new PlayerAskedQuestionAction(processState.CurrentPlayerNum, command.AnsweringPlayerNum, command.Coordinate),
                new PlayerAnsweredToQuestionAction(processState.CurrentPlayerNum, command.AnsweringPlayerNum, answer, command.Coordinate),
            };

            if (answer == true)
            {
                var newTurn = processState.CurrentPlayerNum == processState.PlayerCount - 1 ? processState.CurrentTurn + 1 : processState.CurrentTurn;
                var newOrder = processState.CurrentPlayerNum == processState.PlayerCount - 1
                    ? 0
                    : processState.CurrentPlayerNum + 1;

                var newGrid = processState.CellStateGrid.Copy();
                var oldCell = processState.CellStateGrid.Get(command.Coordinate);
                newGrid.Set(command.Coordinate, 
                    new CellState(
                        oldCell.ImproperCellForPlayer,
                        oldCell.PossibleCellForPlayers.Add(command.AnsweringPlayerNum)
                    )
                );

                return (
                    processState.Set(new ProcessUpdate
                    {
                        Actions = processState.Actions.Concat(actions).ToList(),
                        CellStateGrid = newGrid,
                        CurrentPlayerNum = newOrder,
                        CurrentTurn = newTurn,
                        GameState = new PlayerMakesTurnState(),
                    }),
                    actions);
            }
            else
            {
                var newGrid = processState.CellStateGrid.Copy();
                var oldCell = processState.CellStateGrid.Get(command.Coordinate);
                newGrid.Set(command.Coordinate, 
                    new CellState(
                        command.AnsweringPlayerNum,
                        oldCell.PossibleCellForPlayers
                    )
                );

                return (
                    processState.Set(new ProcessUpdate
                    {
                        Actions = processState.Actions.Concat(actions).ToList(),
                        CellStateGrid = newGrid,
                        GameState = new PlayerPutsImproperCellAfterFailState(),
                    }),
                    actions);
            }
        }
        
        private (ProcessState, List<GameAction>) RunMakeGuessCommandOnPlayerMakesTurnState(ProcessState processState, IGrid<bool[]> possibleCells, MakeGuessCommand command)
        {
            if (possibleCells.Get(command.Coordinate)[processState.CurrentPlayerNum] == false)
            {
                throw new ArgumentException($"Player cannot put just one improper cell on impossible cell ({command.Coordinate})", nameof(command));
            }
            
            if (processState.CellStateGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }
            
            var actions = new List<GameAction>
            {
                new PlayerMadeGuessAction(processState.CurrentPlayerNum, command.Coordinate)
            };
            
            var oldCell = processState.CellStateGrid.Get(command.Coordinate);
            var possibles = oldCell.PossibleCellForPlayers;
            possibles = possibles.Add(processState.CurrentPlayerNum);

            var newGrid = processState.CellStateGrid.Copy();

            for (var num = (processState.CurrentPlayerNum + 1) % processState.PlayerCount; num != processState.CurrentPlayerNum; num = (num + 1) % processState.PlayerCount)
            {
                var answer = possibleCells.Get(command.Coordinate)[num];
                actions.Add(new PlayerAnsweredToGuessAction(processState.CurrentPlayerNum, num, answer, command.Coordinate));
                if (answer == true)
                {
                    possibles = possibles.Add(num);
                }
                else
                {
                    newGrid.Set(command.Coordinate, 
                        new CellState(
                            num,
                            possibles
                        )
                    );
                    return (
                        processState.Set(new ProcessUpdate
                        {
                            Actions = processState.Actions.Concat(actions).ToList(),
                            CellStateGrid = newGrid,
                            GameState = new PlayerPutsImproperCellAfterFailState(),
                        }),
                        actions);
                }
            }
            
            actions.Add(new PlayerWonAction(processState.CurrentPlayerNum));

            newGrid.Set(command.Coordinate, 
                new CellState(
                    oldCell.ImproperCellForPlayer,
                    possibles
                )
            );

            return (
                processState.Set(new ProcessUpdate
                {
                    Actions = processState.Actions.Concat(actions).ToList(),
                    CellStateGrid = newGrid,
                    GameState = new PlayerWinsGameState(),
                }),
                actions);
        }
    
        private (ProcessState, List<GameAction>) RunPutImproperCellAfterFailCommandOnPlayerPutsImproperCellAfterFailState(ProcessState processState, IGrid<bool[]> possibleCells, PutImproperCellAfterFailCommand command)
        {
            if (possibleCells.Get(command.Coordinate)[processState.CurrentPlayerNum] == true)
            {
                throw new ArgumentException($"Player cannot put just one improper cell on possible cell ({command.Coordinate})", nameof(command));
            }
            
            if (processState.CellStateGrid.Get(command.Coordinate).ImproperCellForPlayer.HasValue)
            {
                throw new ArgumentException($"Player can put just one improper cell on coordinate ({command.Coordinate})", nameof(command));
            }

            var actions = new List<GameAction>
            {
                new PlayerPutImproperCellAfterFailAction(processState.CurrentPlayerNum, command.Coordinate)
            };
            
            var newTurn = processState.CurrentPlayerNum == processState.PlayerCount - 1 ? processState.CurrentTurn + 1 : processState.CurrentTurn;
            var newOrder = processState.CurrentPlayerNum == processState.PlayerCount - 1
                ? 0
                : processState.CurrentPlayerNum + 1;

            var newGrid = processState.CellStateGrid.Copy();
            var oldCell = processState.CellStateGrid.Get(command.Coordinate);
            newGrid.Set(command.Coordinate, 
                new CellState(
                    processState.CurrentPlayerNum, 
                    oldCell.PossibleCellForPlayers
                )
            );

            return (
                processState.Set(new ProcessUpdate
                {
                    Actions = processState.Actions.Concat(actions).ToList(),
                    CellStateGrid = newGrid,
                    CurrentPlayerNum = newOrder,
                    CurrentTurn = newTurn,
                    GameState = new PlayerMakesTurnState(),
                }),
                actions);
        }
    }
}