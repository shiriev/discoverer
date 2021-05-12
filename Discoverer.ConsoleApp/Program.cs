using System;
using System.Collections.Generic;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.GameContract;
using Discoverer.Logic.Generator;
using Discoverer.Logic.Grid.Hex;
using Discoverer.Logic.Grid.Isometric;
using Discoverer.Logic.Settings;

namespace Discoverer.ConsoleApp
{
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

            var generator = new LevelGenerator();
            var level = generator.Generate(new GenerationSettings
            {
                GridType = EGridType.Hex,
                Width = 12,
                Height = 12,
                PlayerCount = 5,
            });

            if (level.Grid.Type == HexGrid.GlobalType)
            {
                DrawHex((level.Grid as HexGrid<Cell>).Cells);
            }
            else
            {
                DrawIsometric((level.Grid as IsometricGrid<Cell>).Cells);
            }

            Console.WriteLine($"{level.Grail} [{string.Join(", ", level.Hints)}]");

            //foreach (var (coord, hint) in hints)
            //{
            //    Console.WriteLine($"{coord} [{string.Join(", ", hint)}]");
            //}

            Console.ReadLine();
        }

        static void DrawIsometric(Cell[,] cells)
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
                    DrawCell(cells[x, y]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Y");
        }
        
        static void DrawHex(Cell[,] cells)
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
                    DrawCell(cells[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("X");
        }

        static void DrawCell(Cell cell)
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
        
        static void DrawCell2(Cell cell)
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