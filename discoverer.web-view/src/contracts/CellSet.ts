import Region from "./Region";
import MarkerSet from "./MarkerSet";
import Coordinate from "./Coordinate";

class CellSet 
{
    region: Region;
    markerSet: MarkerSet;
    clickable: boolean;
    usersToAsk: Array<string>;
    coordinate: Coordinate;

    constructor(
        region: Region,
        markerSet: MarkerSet,
        clickable: boolean,
        usersToAsk: Array<string>,
        coordinate: Coordinate)
    {
        this.region = region;
        this.markerSet = markerSet;
        this.clickable = clickable;
        this.usersToAsk = usersToAsk;
        this.coordinate = coordinate;
    }
}

export default CellSet;