using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Hints
{
    public static class HintFunctions
    {
        public static readonly Dictionary<EHint, Func<IGrid<Cell>, ICoordinate, bool>> Functions = new() {
            {EHint.DesertOrForest, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Desert or ETerrainType.Forest},
            {EHint.DesertOrMountain, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Desert or ETerrainType.Mountain},
            {EHint.DesertOrSwamp, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Desert or ETerrainType.Swamp},
            {EHint.DesertOrWater, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Desert or ETerrainType.Water},
            {EHint.ForestOrMountain, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Forest or ETerrainType.Mountain},
            {EHint.ForestOrSwamp, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Forest or ETerrainType.Swamp},
            {EHint.ForestOrWater, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Forest or ETerrainType.Water},
            {EHint.MountainOrSwamp, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Mountain or ETerrainType.Swamp},
            {EHint.MountainOrWater, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Mountain or ETerrainType.Water},
            {EHint.SwampOrWater, (grid, coord) => grid.Get(coord).Terrain is ETerrainType.Swamp or ETerrainType.Water},
            
            {EHint.WithinOneCellOfDesert, (grid, coord) => NearBy(grid, coord, 1, cell => cell.Terrain is ETerrainType.Desert)},
            {EHint.WithinOneCellOfForest, (grid, coord) => NearBy(grid, coord, 1, cell => cell.Terrain is ETerrainType.Forest)},
            {EHint.WithinOneCellOfMountain, (grid, coord) => NearBy(grid, coord, 1, cell => cell.Terrain is ETerrainType.Mountain)},
            {EHint.WithinOneCellOfSwamp, (grid, coord) => NearBy(grid, coord, 1, cell => cell.Terrain is ETerrainType.Swamp)},
            {EHint.WithinOneCellOfWater, (grid, coord) => NearBy(grid, coord, 1, cell => cell.Terrain is ETerrainType.Water)},
            
            {EHint.WithinOneCellOfAnyHabitat, (grid, coord) => NearBy(grid, coord, 1, cell => cell.Habitat is not null)},
            
            {EHint.WithinTwoCellsOfAnyMonument, (grid, coord) => NearBy(grid, coord, 2, cell => cell.Building is {Type: EBuildingType.Monument})},
            {EHint.WithinTwoCellsOfAnyOldHouse, (grid, coord) => NearBy(grid, coord, 2, cell => cell.Building is {Type: EBuildingType.OldHouse})},
            
            {EHint.WithinTwoCellsOfBearHabitat, (grid, coord) => NearBy(grid, coord, 2, cell => cell.Habitat is EHabitatType.Bear)},
            {EHint.WithinTwoCellsOfTigerHabitat, (grid, coord) => NearBy(grid, coord, 2, cell => cell.Habitat is EHabitatType.Tiger)},
            
            {EHint.WithinThreeCellsOfAnyBlackBuilding, (grid, coord) => NearBy(grid, coord, 3, cell => cell.Building is {Color: EColor.Black})},
            {EHint.WithinThreeCellsOfAnyRedBuilding, (grid, coord) => NearBy(grid, coord, 3, cell => cell.Building is {Color: EColor.Red})},
            {EHint.WithinThreeCellsOfAnyBlueBuilding, (grid, coord) => NearBy(grid, coord, 3, cell => cell.Building is {Color: EColor.Blue})},
            {EHint.WithinThreeCellsOfAnyWhiteBuilding, (grid, coord) => NearBy(grid, coord, 3, cell => cell.Building is {Color: EColor.White})},
        };

        public static Dictionary<EHint, Func<IGrid<Cell>, ICoordinate, bool>> SimpleModeFunctions => Functions;
        
        private static bool NearBy(IGrid<Cell> grid, ICoordinate coord, int distance, Func<Cell, bool> rule)
        {
            return grid.NearItems(coord, distance).Any(t => rule(t.Item2));
        }
    }
}