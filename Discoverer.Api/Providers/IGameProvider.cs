using System;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Settings;

namespace Discoverer.Api.Providers;

public interface IGameProvider
{
     GameSave? GetGameState(Guid gameId);

     void SaveGameState(Guid gameId, GameSave gameSave);

     GameSettings? GetGameSettings(Guid gameId);

     void SaveGameSettings(Guid gameId, GameSettings gameSettings);
}