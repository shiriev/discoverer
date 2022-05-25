
class MarkerSet 
{
    improperCellForPlayer: number | null;
    possibleCellForPlayers: Array<number>;

    constructor(
        improperCellForPlayer: number | null,
        possibleCellForPlayers: Array<number>)
    {
        this.improperCellForPlayer = improperCellForPlayer;
        this.possibleCellForPlayers = possibleCellForPlayers;
    }
}

export default MarkerSet;