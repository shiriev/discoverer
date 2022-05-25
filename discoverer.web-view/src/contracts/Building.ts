import Color from "./Color";
import BuildingType from "./BuildingType";

class Building
{
    color: Color;
    buildingType: BuildingType;

    constructor(
        color: Color,
        buildingType: BuildingType)
    {
        this.color = color;
        this.buildingType = buildingType;
    }
}

export default Building;