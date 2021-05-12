using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.CellContract;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Generator
{
    internal class GridGeneratorConstants
    {
        
    }

    internal class GridGenerator
    {
        protected const double TerrainMarkerCountToGridSizeSqrt = 1;
        protected const double HabitatCountToGridSizeSqrt = 1;
        protected static readonly Array TerrainValues = Enum.GetValues(typeof(ETerrainType));
        protected static readonly Building[] DefaultBuilding =
        {
            new() { Color = EColor.Black, Type = EBuildingType.Monument, },
            new() { Color = EColor.Black, Type = EBuildingType.OldHouse, },
            new() { Color = EColor.Red, Type = EBuildingType.Monument, },
            new() { Color = EColor.Red, Type = EBuildingType.OldHouse, },
        };
        
        protected static readonly EHabitatType[] DefaultHabitats =
        {
            EHabitatType.Bear,
            EHabitatType.Tiger,
            EHabitatType.Bear,
            EHabitatType.Tiger,
        };
    }
    
    internal class GridGenerator<TCoord> : GridGenerator where TCoord : ICoordinate
    {
        // TODO: Get counts of object from outside

        private readonly IGridBuilder<TCoord> _gridBuilder;
        private readonly ICoordinateRandom<TCoord> _randomCoordinate;
        private readonly ICoordinateHelper<TCoord> _coordinateHelper;
        private readonly Random _random;

        public GridGenerator(IGridBuilder<TCoord> gridBuilder, Random random)
        {
            _gridBuilder = gridBuilder;
            _random = random;
            _randomCoordinate = _gridBuilder.BuildRandom(random);
            _coordinateHelper = _gridBuilder.BuildCoordinateHelper();
        }

        public IGrid<Cell, TCoord> Generate()
        {
            var terrains = GenerateTerrain();
            var habitats = GenerateHabitat();
            var buildings = GenerateBuildings();

            var grid = _gridBuilder.BuildGrid<Cell>();

            foreach (var (coord, _) in grid.Items)
            {
                grid.Set(coord, new Cell
                {
                    Terrain = terrains.Get(coord),
                    Habitat = habitats.Get(coord),
                    Building = buildings.TryGetValue(coord, out var b) ? b : null,
                });
            }

            return grid;
        }
        
        private IGrid<ETerrainType, TCoord> GenerateTerrain()
        {
            var terrains = _gridBuilder.BuildGrid<ETerrainType>();
            var markers = new Dictionary<TCoord, ETerrainType>();
            var startValues = (int)(TerrainMarkerCountToGridSizeSqrt * Math.Sqrt(terrains.Size));
            
            for (var i = 0; i < startValues; ++i)
            {
                var coord = _randomCoordinate.Next();
                if (markers.ContainsKey(coord))
                {
                    --i;
                }
                else
                {
                    markers[coord] = GetRandomTerrain();
                }
            }

            foreach (var (coord, _) in terrains.Items)
            {
                var minDistance = int.MaxValue;
                var nearestMarkerCoord = markers.First().Key;
                
                foreach (var (markerCoord, _) in markers)
                {
                    var distance = _coordinateHelper.CalculateDistance(coord, markerCoord);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestMarkerCoord = markerCoord;
                    }
                }
                
                terrains.Set(coord, markers[nearestMarkerCoord]);
            }
                
            return terrains;
        }

        private ETerrainType GetRandomTerrain()
        {
            return (ETerrainType)(TerrainValues.GetValue(_random.Next(TerrainValues.Length)) ?? throw new InvalidOperationException());
        }

        private IGrid<EHabitatType?, TCoord> GenerateHabitat()
        {
            var habitats = _gridBuilder.BuildGrid<EHabitatType?>();

            var emptyCellCount = (int)(HabitatCountToGridSizeSqrt * Math.Sqrt(habitats.Size));

            for (var i = 0; i < DefaultHabitats.Length; ++i)
            {
                var coord = _randomCoordinate.Next();
                if (habitats.Get(coord) is not null)
                {
                    --i;
                }
                else
                {
                    habitats.Set(coord, DefaultHabitats[i]);
                    emptyCellCount--;
                }
            }

            while (emptyCellCount > 0)
            {
                var coord = _randomCoordinate.Next();
                if (habitats.Get(coord) is not null)
                {
                    continue;
                }

                var neighbours = habitats.NearItems(coord).Where(_ => _.Item2.HasValue);
                if (neighbours.Any())
                {
                    var (neighbourCoord, neighbourHabitat) = neighbours.First();
                    habitats.Set(coord, neighbourHabitat);
                    emptyCellCount--;
                }
            }

            return habitats;
        }

        private Dictionary<TCoord, Building> GenerateBuildings()
        {
            var buildings = new Dictionary<TCoord, Building>();
            
            for (var i = 0; i < DefaultBuilding.Length; ++i)
            {
                var coord = _randomCoordinate.Next();
                if (buildings.ContainsKey(coord))
                {
                    --i;
                }
                else
                {
                    buildings[coord] = DefaultBuilding[i];
                }
            }

            return buildings;
        }
    }
}