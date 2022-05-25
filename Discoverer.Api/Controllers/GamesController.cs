using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discoverer.Api.Controllers.Contracts;
using Discoverer.Api.Providers;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Commands;
using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Contracts.GameStates;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;
using Discoverer.Logic.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Discoverer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly ILogger<GamesController> _logger;
    private readonly GameBuilder _gameBuilder;
    private readonly IGameProvider _gameProvider;
        
    public GamesController(
        ILogger<GamesController> logger,
        GameBuilder gameBuilder,
        IGameProvider gameProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _gameBuilder = gameBuilder ?? throw new ArgumentNullException(nameof(gameBuilder));
        _gameProvider = gameProvider ?? throw new ArgumentNullException(nameof(gameProvider));
    }
        
    [HttpPost("create")]
    public async Task<ActionResult<StartGameResponse>> CreateNewGame([FromBody] StartGameRequest startGameRequest)
    {
        _logger.LogInformation(JsonSerializer.Serialize(startGameRequest));
        if (startGameRequest?.GameSettings is null)
        {
            return BadRequest();
        }

        var game = _gameBuilder.Create(startGameRequest.GameSettings);
        _gameProvider.SaveGameState(startGameRequest.GameId, game.SaveState());
        _gameProvider.SaveGameSettings(startGameRequest.GameId, startGameRequest.GameSettings);

        return Ok(new StartGameResponse());
    }

    [HttpGet("state")]
    public async Task<ActionResult<GameStateResponse>> GetGameState([FromQuery] Guid gameId)
    {
        _logger.LogInformation(JsonSerializer.Serialize(gameId));
        if (gameId == Guid.Empty)
        {
            return BadRequest();
        }

        var save = _gameProvider.GetGameState(gameId);
        var settings = _gameProvider.GetGameSettings(gameId);

        if (save is null || settings is null)
        {
            return BadRequest();
        }
        
        var game = _gameBuilder.Create(settings);
        game.LoadState(save);
        var regionGrid = GetList(settings.GridType == EGridType.Hex
            ? game.Hex.RegionGrid.Cells
            : game.Isometric.RegionGrid.Cells);
        var markerSetGrid = GetList(settings.GridType == EGridType.Hex
            ? game.Hex.MarkerSetGrid.Cells
            : game.Isometric.MarkerSetGrid.Cells);
        var gameState = new GameStateResponse
        {
            GameState = ToEnum(game.GetGameState()),
            AllActions = game.GetAllActions().ToList(),
            CurrentPlayerNum = game.GetCurrentPlayer(),
            PlayerCount = settings.PlayerCount,
            CurrentTurn = game.GetCurrentTurn(),
            RegionGrid = regionGrid,
            MarkerSetGrid = markerSetGrid,
            GridType = settings.GridType,
        };

        return Ok(gameState);
    }

    [HttpGet("possible-command")]
    public async Task<ActionResult<List<CommandTypeAndCommands>>> GetPossibleCommands([FromQuery] Guid gameId, [FromQuery] int playerNum)
    {
        _logger.LogInformation(JsonSerializer.Serialize(gameId));
        if (gameId == Guid.Empty)
        {
            return BadRequest();
        }

        var save = _gameProvider.GetGameState(gameId);
        var settings = _gameProvider.GetGameSettings(gameId);

        if (save is null || settings is null)
        {
            return BadRequest();
        }
        
        var game = _gameBuilder.Create(settings);
        game.LoadState(save);

        if (game.GetCurrentPlayer() != playerNum)
        {
            return BadRequest();
        }

        var possibleCommands = game.GetCurrentPossibleCommands().Select(t => new CommandTypeAndCommands
        {
            CommandType = t.CommandType,
            Commands = t.Commands,
        }).ToList();

        return Ok(possibleCommands);
    }

    [HttpPost("command")]
    public async Task<ActionResult> RunCommand([FromQuery] Guid gameId, [FromQuery] int playerNum, [FromBody] JObject gameCommandObj)
    {
        _logger.LogInformation(JsonSerializer.Serialize(gameId));
        if (gameId == Guid.Empty)
        {
            return BadRequest();
        }

        var save = _gameProvider.GetGameState(gameId);
        var settings = _gameProvider.GetGameSettings(gameId);

        if (save is null || settings is null)
        {
            return BadRequest();
        }
        
        var game = _gameBuilder.Create(settings);
        game.LoadState(save);

        if (game.GetCurrentPlayer() != playerNum)
        {
            return BadRequest();
        }

        var command = ParseCommand(gameCommandObj, game.GetGridType());

        if (command is null)
        {
            return BadRequest();
        }

        var actions = game.RunCommand(command);

        var newSave = game.SaveState();
        _gameProvider.SaveGameState(gameId, newSave);

        return Ok();
    }

    // TODO: use enum instead of class
    private EGameState ToEnum(GameState gameState)
    {
        return gameState switch
        {
            GameNotStartedState state => EGameState.GameNotStarted,
            PlayerMakesTurnState state => EGameState.PlayerMakesTurn,
            PlayerPutsImproperCellOnStartState state => EGameState.PlayerPutsImproperCellOnStart,
            PlayerPutsImproperCellAfterFailState state => EGameState.PlayerPutsImproperCellAfterFail,
            PlayerWinsGameState state => EGameState.PlayerWinsGame,
            _ => throw new Exception() // TODO: Change exception
        };
    }

    private List<List<T>> GetList<T>(T[,] array)
    {
        var list = new List<List<T>>(array.GetLength(0));
        for (int i = 0; i < array.GetLength(0); i++)
        {
            var subList = new List<T>(array.GetLength(1));
            for (int j = 0; j < array.GetLength(1); j++)
            {
                subList.Add(array[i, j]);
            }
            list.Add(subList);
        }

        return list;
    }

    private GameCommand? ParseCommand(JObject commandObj, EGridType gridType)
    {
        if (commandObj.Type != JTokenType.Object)
        {
            return null;
        }

        var hasType = commandObj.TryGetValue(nameof(GameCommand.Type), StringComparison.OrdinalIgnoreCase, out var typeObj);

        if (!hasType || typeObj is null)
        {
            return null;
        }

        EGameCommand enumType;
        
        if (typeObj.Type == JTokenType.String)
        {
            var type = typeObj.Value<string>();
            var hasParse = Enum.TryParse(type, out enumType);
            if (!hasParse)
            {
                return null;
            }
        }
        else if (typeObj.Type == JTokenType.Integer)
        {
            enumType = (EGameCommand)typeObj.Value<int>();
            if (!Enum.IsDefined(enumType))
            {
                return null;
            }
        }
        else
        {
            return null;
        }
        // TODO: check nulls
        return enumType switch
        {
            EGameCommand.AskQuestion => new AskQuestionCommand
            (
                AnsweringPlayerNum: commandObj.GetValue(nameof(AskQuestionCommand.AnsweringPlayerNum), StringComparison.OrdinalIgnoreCase).ToObject<int>(),
                Coordinate: ParseCoordinate(gridType, commandObj.GetValue(nameof(AskQuestionCommand.Coordinate), StringComparison.OrdinalIgnoreCase))
            ),
            EGameCommand.MakeGuess => new MakeGuessCommand(ParseCoordinate(gridType, commandObj.GetValue(nameof(MakeGuessCommand.Coordinate), StringComparison.OrdinalIgnoreCase))),
            EGameCommand.StartGame => new StartGameCommand(),
            EGameCommand.PutImproperCellAfterFail => new PutImproperCellAfterFailCommand(ParseCoordinate(gridType, commandObj.GetValue(nameof(PutImproperCellAfterFailCommand.Coordinate), StringComparison.OrdinalIgnoreCase))),
            EGameCommand.PutImproperCellOnStart => new PutImproperCellOnStartCommand(ParseCoordinate(gridType, commandObj.GetValue(nameof(PutImproperCellOnStartCommand.Coordinate), StringComparison.OrdinalIgnoreCase))),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    // TODO: Add correct validation
    private ICoordinate ParseCoordinate(EGridType gridType, JToken coordinateObj)
    {
        if (gridType == EGridType.Hex)
        {
            return coordinateObj.ToObject<HexCoordinate>() ?? throw new ArgumentException();
        }
        else if (gridType == EGridType.Isometric)
        {
            return coordinateObj.ToObject<IsometricCoordinate>() ?? throw new ArgumentException();
        }
        else
        {
            throw new ArgumentException($"Unknown cooridnate type - {Enum.GetName(gridType)}");
        }
    }
}