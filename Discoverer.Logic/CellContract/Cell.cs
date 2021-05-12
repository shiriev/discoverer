namespace Discoverer.Logic.CellContract
{
    // TODO: Try to use Records
    public class Cell
    {
        public ETerrainType Terrain { get; set; }
        
        public EHabitatType? Habitat { get; set; }
        
        public Building? Building { get; set; }
    }
}