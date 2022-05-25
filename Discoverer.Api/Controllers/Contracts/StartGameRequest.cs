using System;
using Discoverer.Logic.Settings;

namespace Discoverer.Api.Controllers.Contracts;

public class StartGameRequest
{
    public Guid GameId { get; set; }
        
    public GameSettings? GameSettings { get; set; }
    
    public string[] PlayerSecrets { get; set; }
}