using System;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Settings;

namespace Discoverer.Api.Providers;

public class GameProvider : IGameProvider
{
    private static GameSave? _gameSave;
    private static GameSettings? _gameSettings;

    public GameSave? GetGameState(Guid gameId)
    {
        return _gameSave;
    }

    public void SaveGameState(Guid gameId, GameSave gameSave)
    {
        _gameSave = gameSave;
    }
    
    public GameSettings? GetGameSettings(Guid gameId)
    {
        return _gameSettings;
    }

    public void SaveGameSettings(Guid gameId, GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
    }
}