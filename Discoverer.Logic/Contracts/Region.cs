using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Logic.Contracts
{
    public record Region(ETerrainType Terrain, EHabitatType? Habitat, Building? Building);
}