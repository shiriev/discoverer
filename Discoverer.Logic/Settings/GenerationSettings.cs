using Discoverer.Logic.GameContract;

namespace Discoverer.Logic.Settings
{
    public class GenerationSettings
    {
        public int PlayerCount { get; set; }
        
        public int Width { get; set; }
        
        public int Height { get; set; }
        
        public EGridType GridType { get; set; }
        
        public int? Seed { get; set; }
    }
}