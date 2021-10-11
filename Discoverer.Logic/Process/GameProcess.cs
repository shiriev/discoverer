﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.GameContract.Commands;
using Discoverer.Logic.GameContract.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Process
{
    public partial class GameProcess : IGameProcess
    {
        private Level _level;
        private ProcessState _processState;
        private IGrid<bool[]> _possibleCells;
        private GameSettings _gameSettings;
        private readonly IGridBuilder _gridBuilder;
        private readonly IProcessUpdater _processUpdater;
        private readonly Dictionary<EHint, Func<IGrid<Cell>, ICoordinate, bool>> _hintFunctions;
        
        // TODO: Make tests
        // TODO: Complete class
        // TODO: Make ctor instead of Init and LoadState
        public GameProcess(IGridBuilder gridBuilder, IProcessUpdater processUpdater, Dictionary<EHint, Func<IGrid<Cell>, ICoordinate, bool>> hintFunctions)
        {
            _gridBuilder = gridBuilder;
            _processUpdater = processUpdater;
            _hintFunctions = hintFunctions;
        }

        public GameCast SaveState()
        {
            return new GameCast
            {
                Level = _level,
                ProcessState = _processState,
                GameSettings = _gameSettings
            };
        }

        public void LoadState(GameCast gameCast)
        {
            _level = gameCast.Level;
            _gameSettings = gameCast.GameSettings;
            _processState = gameCast.ProcessState;
            
            InitPossibleCells();
        }
        
        public bool GetCommandPossibility(GameCommand command)
        {
            return _processUpdater.GetCommandPossibility(_processState, _possibleCells, command);
        }

        public List<GameAction> RunCommand(GameCommand command)
        {
            var (processState, actions) = _processUpdater.RunCommand(_processState, _possibleCells, command);
            _processState = processState;
            return actions;
        }

        public void Init(Level level, GameSettings gameSettings)
        {
            _level = level;
            _gameSettings = gameSettings;
            var grid = _gridBuilder.BuildGrid<CellState>();
            foreach (var (coordinate, cellState) in grid.Items)
            {
                grid.Set(coordinate, new CellState(null, ImmutableHashSet<int>.Empty));
            }

            _processState = new ProcessState(
                new List<GameAction>(), 
                grid, 
                0, 
                0, 
                new GameNotStartedState(), 
                level.Hints.Length, 
                Guid.NewGuid());
            
            InitPossibleCells();
        }

        public IGrid<bool> GetPossibleCellsFor(int playerNum)
        {
            var playerPossibleCells = _gridBuilder.BuildGrid<bool>();
            
            foreach (var (coordinate, bools) in _possibleCells.Items)
            {
                playerPossibleCells.Set(
                    coordinate,
                    bools[playerNum]
                );
            }

            return playerPossibleCells;
        }

        public GameState GetGameState()
        {
            return _processState.GameState;
        }

        public List<GameAction> GetAllActions()
        {
            return _processState.Actions;
        }

        /*public IGrid<(Cell, CellState)> GetGrid()
        {
            var grid = _gridBuilder.BuildGrid<(Cell, CellState)>();
            
            foreach (var (coordinate, cellState) in _processState.CellStateGrid.Items)
            {
                grid.Set(
                    coordinate,
                    (_level.Grid.Get(coordinate), cellState)
                );
            }

            return grid;
        }*/

        public IGrid<Cell> GetCellGrid()
        {
            return _level.Grid;
        }
        
        public IGrid<CellState> GetCellStateGrid()
        {
            return _processState.CellStateGrid;
        }

        public int GetCurrentPlayer()
        {
            return _processState.CurrentPlayerNum;
        }

        public List<Type> GetCurrentPossibleCommands()
        {
            return _processState.GameState switch
            {
                GameNotStartedState state => new List<Type> {typeof(StartGameCommand)},
                PlayerMakesTurnState state => new List<Type> {typeof(AskQuestionCommand), typeof(MakeGuessCommand)},
                PlayerPutsImproperCellOnStartState state => new List<Type> {typeof(PutImproperCellOnStartCommand)},
                PlayerPutsImproperCellAfterFailState state => new List<Type> {typeof(PutImproperCellAfterFailCommand)},
                PlayerWinsGameState state => new List<Type> { },
                _ => throw new Exception() // TODO: Change exception
            };
        }

        public int GetCurrentTurn()
        {
            return _processState.CurrentTurn;
        }

        public ICoordinate GetGrail()
        {
            return _level.Grail;
        }
        
        public EHint[] GetHints()
        {
            return _level.Hints;
        }

        //TODO: Fill grid type
        public EGridType GetGridType()
        {
            return _gameSettings.GridType;
        }
        
        //TODO: Remove method
        private void InitPossibleCells()
        {
            _possibleCells = _gridBuilder.BuildGrid<bool[]>();

            foreach (var (coordinate, _) in _level.Grid.Items)
            {
                _possibleCells.Set(
                    coordinate, 
                    Enumerable.Range(0, _processState.PlayerCount)
                        .Select(i => _hintFunctions[_level.Hints[i]](_level.Grid, coordinate))
                        .ToArray()
                );
            }
        }
    }
}