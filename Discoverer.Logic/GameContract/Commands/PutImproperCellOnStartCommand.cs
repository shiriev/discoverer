﻿using Discoverer.Logic.Grid;

namespace Discoverer.Logic.GameContract.Commands
{
    public record PutImproperCellOnStartCommand(
        ICoordinate Coordinate) : GameCommand;
}