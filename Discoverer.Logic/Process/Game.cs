using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Commands;
using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Contracts.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Process.Contracts;
using Discoverer.Logic.Settings;

namespace Discoverer.Logic.Process
{
    public partial class Game : IGame
    {
        private Level _level;
        private GameCast _gameCast;
        private IGrid<bool[]> _possibleCells;
        private GameSettings _gameSettings;
        private readonly IGridBuilder _gridBuilder;
        private readonly IGameStateUpdater _gameStateUpdater;
        private readonly Dictionary<EHint, Func<IImmutableGrid<Region>, ICoordinate, bool>> _hintFunctions;
        
        // TODO: Make tests
        // TODO: Complete class
        // TODO: Make ctor instead of Init and LoadState
        internal Game(IGridBuilder gridBuilder, IGameStateUpdater gameStateUpdater, Dictionary<EHint, Func<IImmutableGrid<Region>, ICoordinate, bool>> hintFunctions)
        {
            _gridBuilder = gridBuilder;
            _gameStateUpdater = gameStateUpdater;
            _hintFunctions = hintFunctions;
        }

        public GameSave SaveState()
        {
            return new GameSave
            {
                Grid = _level.Grid,
                Hints = _level.Hints,
                Grail = _level.Grail,
                Actions = _gameCast.Actions,
                MarkerSetGrid = _gameCast.MarkerSetGrid,
                CurrentPlayerNum = _gameCast.CurrentPlayerNum,
                CurrentTurn = _gameCast.CurrentTurn,
                GameState = _gameCast.GameState,
                PlayerCount = _gameCast.PlayerCount,
                GameId = _gameCast.GameId,
                GameSettings = _gameSettings
            };
        }

        public void LoadState(GameSave gameSave)
        {
            _level = new Level
            {
                Grid = _level.Grid,
                Hints = _level.Hints,
                Grail = _level.Grail
            };
            _gameCast = new GameCast(
                Actions: _gameCast.Actions,
                MarkerSetGrid: _gameCast.MarkerSetGrid,
                CurrentPlayerNum: _gameCast.CurrentPlayerNum,
                CurrentTurn: _gameCast.CurrentTurn,
                GameState: _gameCast.GameState,
                PlayerCount: _gameCast.PlayerCount,
                GameId: _gameCast.GameId
            );
            _gameSettings = gameSave.GameSettings;
            
            InitPossibleCells();
        }
        
        public bool GetCommandPossibility(GameCommand command)
        {
            return _gameStateUpdater.GetCommandPossibility(_gameCast, _possibleCells, command);
        }

        public ImmutableList<GameAction> RunCommand(GameCommand command)
        {
            var (gameCast, actions) = _gameStateUpdater.RunCommand(_gameCast, _possibleCells, command);
            _gameCast = gameCast;
            return actions;
        }

        internal void Init(Level level, GameSettings gameSettings)
        {
            _level = level;
            _gameSettings = gameSettings;
            var grid = _gridBuilder.BuildGrid<MarkerSet>();
            foreach (var (coordinate, cellState) in grid.Items)
            {
                grid.Set(coordinate, new MarkerSet(null, ImmutableHashSet<int>.Empty));
            }

            _gameCast = new GameCast(
                ImmutableList<GameAction>.Empty, 
                grid.ToImmutable(), 
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
            return _gameCast.GameState;
        }

        public ImmutableList<GameAction> GetAllActions()
        {
            return _gameCast.Actions;
        }
        
        public IGrid<Region> GetRegionGrid()
        {
            return _level.Grid.CopyGrid();;
        }
        
        public IGrid<MarkerSet> GetMarkerSetGrid()
        {
            return _gameCast.MarkerSetGrid.CopyGrid();
        }

        public int GetCurrentPlayer()
        {
            return _gameCast.CurrentPlayerNum;
        }

        public List<Type> GetCurrentPossibleCommands()
        {
            return _gameCast.GameState switch
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
            return _gameCast.CurrentTurn;
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
                    Enumerable.Range(0, _gameCast.PlayerCount)
                        .Select(i => _hintFunctions[_level.Hints[i]](_level.Grid, coordinate))
                        .ToArray()
                );
            }
        }
    }
}