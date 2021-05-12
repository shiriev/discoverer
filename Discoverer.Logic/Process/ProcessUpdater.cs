using System;
using System.Collections.Generic;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.GameContract.Actions;
using Discoverer.Logic.GameContract.Commands;
using Discoverer.Logic.GameContract.GameStates;

namespace Discoverer.Logic.Process
{
    internal class ProcessUpdater
    {
        public (ProcessState, List<GameAction>) RunCommand(ProcessState processState, Level level, GameCommand command)
        {
            if (command is StartGameCommand)
            {
                if (processState.GameState is GameNotStartedState)
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
                else
                {
                    throw new ArgumentException($"You cannot run {typeof(StartGameCommand)} when game on {processState.GameState.GetType().Name} state", nameof(command));
                }
            } else
            {
                throw new ArgumentException($"You cannot run {command.GetType().Name}", nameof(command));
            }
        }
    }
}