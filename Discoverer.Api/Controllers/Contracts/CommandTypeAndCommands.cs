using System.Collections.Generic;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Api.Controllers.Contracts;

public class CommandTypeAndCommands
{
    public EGameCommand CommandType { get; set; }
    
    public List<GameCommand> Commands { get; set; }
}