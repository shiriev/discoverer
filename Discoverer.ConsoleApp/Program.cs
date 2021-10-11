using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.GameContract.Actions;
using Discoverer.Logic.GameContract.Commands;
using Discoverer.Logic.GameContract.GameStates;
using Discoverer.Logic.Generator;
using Discoverer.Logic.Grid;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;
using Discoverer.Logic.Hints;
using Discoverer.Logic.Process;
using Discoverer.Logic.Settings;

namespace Discoverer.ConsoleApp
{
    #nullable enable
    class Program
    {
        private static Dictionary<ETerrainType, ConsoleColor> colors = new()
        {
            {ETerrainType.Desert, ConsoleColor.Yellow},
            {ETerrainType.Forest, ConsoleColor.DarkGreen},
            {ETerrainType.Mountain, ConsoleColor.Gray},
            {ETerrainType.Swamp, ConsoleColor.DarkMagenta},
            {ETerrainType.Water, ConsoleColor.Blue},
        };
        private static Dictionary<EColor, ConsoleColor> colorToColor = new()
        {
            {EColor.Black, ConsoleColor.White},
            {EColor.Red, ConsoleColor.DarkRed},
            {EColor.Blue, ConsoleColor.Blue},
            {EColor.White, ConsoleColor.Black},
        };
        private static Dictionary<EBuildingType, string> buildingToString = new()
        {
            {EBuildingType.Monument, "M"},
            {EBuildingType.OldHouse, "H"},
        };
        
        static void Main(string[] args)
        {
            // TODO: Добавить тесты на основные алгоритмы
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var gameBuilder = new GameBuilder();
            var game = gameBuilder.Create(new GameSettings
            {
                GridType = EGridType.Hex,
                Width = 5,
                Height = 5,
                PlayerCount = 2,
            });
            game.RunCommand(new StartGameCommand());
            
            while (true)
            {
                Draw(game);

                var possibleCommands = game.GetCurrentPossibleCommands();

                var command = BuildCommand(possibleCommands, game.GetGridType());
                if (command is null)
                    continue;

                if (!game.GetCommandPossibility(command))
                {
                    Console.WriteLine("Невозможно выполнить команду");
                    continue;
                }

                var actions = game.RunCommand(command);

                PrintActions(game.GetAllActions());
            }

            
            Console.ReadLine();
        }

        static GameCommand? BuildCommand(List<Type> possibleCommands, EGridType gridType)
        {
            foreach (var possibleCommand in possibleCommands)
            {
                Console.WriteLine(possibleCommand.Name);
            }

            var commandString = Console.ReadLine();
            var parts = commandString.Split(" ");
            return parts[0] switch
            {
                "ask" => new AskQuestionCommand(int.Parse(parts[1]), gridType == EGridType.Hex 
                    ? new HexCoordinate { X = int.Parse(parts[2]), Y = int.Parse(parts[3]) }
                    : new IsometricCoordinate { X = int.Parse(parts[2]), Y = int.Parse(parts[3]) }),
                "guess" => new MakeGuessCommand(gridType == EGridType.Hex 
                    ? new HexCoordinate { X = int.Parse(parts[1]), Y = int.Parse(parts[2]) }
                    : new IsometricCoordinate { X = int.Parse(parts[1]), Y = int.Parse(parts[2]) }),
                "start" => new StartGameCommand(),
                "put" when possibleCommands.Contains(typeof(PutImproperCellOnStartCommand)) => new PutImproperCellOnStartCommand(gridType == EGridType.Hex 
                    ? new HexCoordinate { X = int.Parse(parts[1]), Y = int.Parse(parts[2]) }
                    : new IsometricCoordinate { X = int.Parse(parts[1]), Y = int.Parse(parts[2]) }),
                "put" when possibleCommands.Contains(typeof(PutImproperCellAfterFailCommand)) => new PutImproperCellAfterFailCommand(gridType == EGridType.Hex 
                    ? new HexCoordinate { X = int.Parse(parts[1]), Y = int.Parse(parts[2]) }
                    : new IsometricCoordinate { X = int.Parse(parts[1]), Y = int.Parse(parts[2]) }),
                _ => null
            };
        }

        static void Draw(GameProcess game)
        {
            if (game.GetGridType() == EGridType.Hex)
            {
                DrawHex(game.Hex.RegionGrid.Cells, DrawRegion);
                DrawHex(game.Hex.CellStateGrid.Cells, DrawCellState);
            
                Console.WriteLine($"{game.Hex.Grail} [{string.Join(", ", game.GetHints())}]");

            }
            else
            {
                DrawIsometric(game.Isometric.RegionGrid.Cells, DrawRegion);
                DrawIsometric(game.Isometric.CellStateGrid.Cells, DrawCellState);
            
                Console.WriteLine($"{game.Isometric.Grail} [{string.Join(", ", game.GetHints())}]");

            }
            //foreach (var (coord, hint) in hints)
            //{
            //    Console.WriteLine($"{coord} [{string.Join(", ", hint)}]");
            //}

        }
        
        static void DrawIsometric<T>(T[,] regions, Action<T> draw)
        {
            var (width, height) = (regions.GetLength(0), regions.GetLength(1));
            
            
            Console.Write("  ");
            for (int x = 0; x < width; ++x)
            {
                Console.Write($"{x:00} ");
            }
            Console.WriteLine("X");

            for (int y = 0; y < height; ++y)
            {
                Console.Write($"{y:00}");
                for (int x = 0; x < width; ++x)
                {
                    draw(regions[x, y]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Y");
        }
        
        static void DrawHex<T>(T[,] regions, Action<T> draw)
        {
            var (width, height) = (regions.GetLength(0), regions.GetLength(1));
            
            Console.Write("  ");
            for (int y = 0; y < height; y += 2)
            {
                Console.Write($"{y:00}  ");
            }
            Console.WriteLine();
            
            Console.Write("    ");
            for (int y = 1; y < height; y += 2)
            {
                Console.Write($"{y:00}  ");
            }
            Console.WriteLine("Y");
            
            for (int x = 0; x < width; ++x)
            {
                Console.Write($"{x:00}");
                if (x % 2 == 1)
                    Console.Write(" ");
                for (int y = 0; y < height; ++y)
                {
                    draw(regions[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("X");
        }
        
        static void DrawIsometric2((Region, CellState)[,] cells)
        {
            var (width, height) = (cells.GetLength(0), cells.GetLength(1));
            
            
            Console.Write("  ");
            for (int x = 0; x < width; ++x)
            {
                Console.Write($"{x:00} ");
            }
            Console.WriteLine("X");

            for (int y = 0; y < height; ++y)
            {
                Console.Write($"{y:00}");
                for (int x = 0; x < width; ++x)
                {
                    DrawRegion(cells[x, y].Item1);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Y");
        }
        
        static void DrawHex2((Region, CellState)[,] cells)
        {
            var (width, height) = (cells.GetLength(0), cells.GetLength(1));
            
            Console.Write("  ");
            for (int y = 0; y < height; y += 2)
            {
                Console.Write($"{y:00}  ");
            }
            Console.WriteLine();
            
            Console.Write("    ");
            for (int y = 1; y < height; y += 2)
            {
                Console.Write($"{y:00}  ");
            }
            Console.WriteLine("Y");
            
            for (int x = 0; x < width; ++x)
            {
                Console.Write($"{x:00}");
                if (x % 2 == 1)
                    Console.Write(" ");
                for (int y = 0; y < height; ++y)
                {
                    DrawRegion(cells[x, y].Item1);
                }
                Console.WriteLine();
            }
            Console.WriteLine("X");
        }

        static void DrawRegion(Region cell)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = colors[cell.Terrain];
            if (cell.Habitat is not null || cell.Building is not null)
            {
                if (cell.Habitat is not null)
                {
                    Console.ForegroundColor = cell.Habitat == EHabitatType.Bear ? ConsoleColor.White : ConsoleColor.DarkRed;
                    Console.Write(cell.Habitat == EHabitatType.Bear ? "b" : "t");
                }
                else
                {
                    Console.Write(" ");
                }
                if (cell.Building is not null)
                {
                    Console.ForegroundColor = colorToColor[cell.Building.Color];
                    Console.Write(buildingToString[cell.Building.Type]);
                }
                else
                {
                    Console.Write(" ");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" ");
                Console.Write(" ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.Write(mapper[map[x, y]] + " ");
        }
        
        static void DrawCellState(CellState cellState)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            if (cellState.ImproperCellForPlayer.HasValue)
            {
                Console.Write(cellState.ImproperCellForPlayer);
            }
            else
            {
                Console.Write(" ");
            }
            if (!cellState.PossibleCellForPlayers.IsEmpty)
            {
                Console.Write(cellState.PossibleCellForPlayers.Min());
            }
            else
            {
                Console.Write(" ");
            }
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.BackgroundColor = ConsoleColor.Black;
            
            //Console.Write(mapper[map[x, y]] + " ");
        }

        static void PrintActions(IEnumerable<GameAction> gameActions)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var messages = gameActions.Select(action => action switch
            {
                GameStartedAction => "Игра началась",
                PlayerPutImproperCellOnStartAction concreteAction 
                    => $"Игрок {concreteAction.PlayerNum} отметил что в точке {concreteAction.Cell} нет объекта",
                PlayerAskedQuestionAction concreteAction 
                    => $"Игрок {concreteAction.AskingPlayerNum} задал вопрос игроку {concreteAction.AnsweringPlayerNum} по поводу точки {concreteAction.Cell}",
                PlayerAnsweredToQuestionAction concreteAction
                    => $"Игрок {concreteAction.AnsweringPlayerNum} ответил игроку {concreteAction.AskingPlayerNum} что точка {concreteAction.Cell} {(concreteAction.Answer ? "возможна" : "невозможна")}",
                PlayerMadeGuessAction concreteAction 
                    => $"Игрок {concreteAction.PlayerNum} предполагает что ответ в точке {concreteAction.Cell}",
                PlayerAnsweredToGuessAction concreteAction 
                    => $"Игрок {concreteAction.AnsweringPlayerNum} ответил игроку {concreteAction.AskingPlayerNum} что точка {concreteAction.Cell} {(concreteAction.Answer ? "возможна" : "невозможна")}",
                PlayerPutImproperCellAfterFailAction concreteAction 
                    => $"Игрок {concreteAction.PlayerNum} отметил что в точке {concreteAction.Cell} нет объекта",
                PlayerWonAction concreteAction 
                    => $"Игрок {concreteAction.PlayerNum} выиграл",
                _ => throw new ArgumentException()
            });

            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }

        static void DrawCell2(Region cell)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = colors[cell.Terrain];
            if (cell.Habitat is not null)
            {
                Console.Write(cell.Habitat == EHabitatType.Bear ? "b" : "t");
            }
            else
            {
                Console.Write(" ");
            }
            if (cell.Building is not null)
            {
                Console.ForegroundColor = colorToColor[cell.Building.Color];
                Console.Write(buildingToString[cell.Building.Type]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            //Console.Write(mapper[map[x, y]] + " ");
        }
    }
}