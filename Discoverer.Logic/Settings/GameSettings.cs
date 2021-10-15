using Discoverer.Logic.Contracts.Enums;

namespace Discoverer.Logic.Settings
{
    public class GameSettings
    {
        public int PlayerCount { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public EGridType GridType { get; set; }
        
        // TODO: Make decision - Drop or not seed
        public int? Seed { get; set; }
    }
}