import Building from "./Building";
import HabitatType from "./HabitatType";
import TerrainType from "./TerrainType";

class Region
{
    terrain: TerrainType;
    habitat: HabitatType | null;
    building: Building;

    constructor(
        terrain: TerrainType,
        habitat: HabitatType | null,
        building: Building)
    {
        this.terrain = terrain;
        this.habitat = habitat;
        this.building = building;
    }
}

export default Region;