using System;
using System.Collections.Generic;
using System.Linq;
using Discoverer.Logic.Contracts;
using Discoverer.Logic.Contracts.Enums;
using Discoverer.Logic.Grid;

namespace Discoverer.Logic.Generator
{
    internal sealed class GridGeneratorConstants
    {
        
    }

    internal sealed class GridGenerator
    {
        // TODO: Get counts of object from outside
        
        protected const double TerrainMarkerCountToGridSizeSqrt = 1;
        protected const double HabitatCountToGridSizeSqrt = 1;
        protected static readonly Array TerrainValues = Enum.GetValues(typeof(ETerrainType));
        protected static readonly Building[] DefaultBuilding =
        {
            new( Color: EColor.Black, Type: EBuildingType.Monument ),
            new( Color: EColor.Black, Type: EBuildingType.OldHouse ),
            new( Color: EColor.Red, Type: EBuildingType.Monument ),
            new( Color: EColor.Red, Type: EBuildingType.OldHouse ),
        };
        
        protected static readonly EHabitatType[] DefaultHabitats =
        {
            EHabitatType.Bear,
            EHabitatType.Tiger,
            EHabitatType.Bear,
            EHabitatType.Tiger,
        };

        private readonly IGridBuilder _gridBuilder;
        private readonly ICoordinateRandom _randomCoordinate;
        private readonly ICoordinateHelper _coordinateHelper;
        private readonly Random _random;

        public GridGenerator(IGridBuilder gridBuilder, Random random)
        {
            _gridBuilder = gridBuilder;
            _random = random;
            _randomCoordinate = _gridBuilder.BuildRandom(random);
            _coordinateHelper = _gridBuilder.BuildCoordinateHelper();
        }

        public IGrid<Region> Generate()
        {
            var terrains = GenerateTerrain();
            var habitats = GenerateHabitat();
            var buildings = GenerateBuildings();

            var grid = _gridBuilder.BuildGrid<Region>();

            foreach (var (coord, _) in grid.Items)
            {
                grid.Set(coord, new Region
                (
                    Terrain: terrains.Get(coord),
                    Habitat: habitats.Get(coord),
                    Building: buildings.TryGetValue(_coordinateHelper.GetUniqueCode(coord), out var t) 
                        ? t.Item2 : null
                ));
            }

            return grid;
        }
        
        private IGrid<ETerrainType> GenerateTerrain()
        {
            var terrains = _gridBuilder.BuildGrid<ETerrainType>();
            var markers = new Dictionary<ICoordinate, ETerrainType>();
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

        private IGrid<EHabitatType?> GenerateHabitat()
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

                var neighbours = habitats.NearItems(coord);
                var firstHabitat = neighbours.Select(n => n.Item2).FirstOrDefault(h => h.HasValue);
                if (firstHabitat is not null)
                {
                    habitats.Set(coord, firstHabitat);
                    emptyCellCount--;
                }
            }

            return habitats;
        }

        private Dictionary<string, (ICoordinate, Building)> GenerateBuildings()
        {
            var buildings = new Dictionary<string, (ICoordinate, Building)>();
            
            for (var i = 0; i < DefaultBuilding.Length; ++i)
            {
                var coord = _randomCoordinate.Next();
                var code = _coordinateHelper.GetUniqueCode(coord);
                if (buildings.ContainsKey(code))
                {
                    --i;
                }
                else
                {
                    buildings[code] = (coord, DefaultBuilding[i]);
                }
            }

            return buildings;
        }
    }
}