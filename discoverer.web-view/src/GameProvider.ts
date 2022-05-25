import { Region, MarkerSet, GameState, GridType } from './contracts';
import { GameAction } from './contracts/Actions';

export type StateResponse = {
    gameState: GameState,
    allActions: Array<GameAction>,
    currentPlayerNum: number,
    playerCount: number,
    currentTurn: number,
    regionGrid: Array<Array<Region>>,
    markerSetGrid: Array<Array<MarkerSet>>,
    gridType: GridType,
}


class GameProvider
{
    uri: string;

    constructor(uri: string)
    {
        this.uri = uri;
    }

    async getState(gameId: string): Promise<StateResponse>
    {
        const response = await fetch(this.uri + `/state?gameId=${gameId}`);
        return await response.json();
    }
}

export default GameProvider;