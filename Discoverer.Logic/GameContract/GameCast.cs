using Discoverer.Logic.Settings;

namespace Discoverer.Logic.GameContract
{
    public class GameCast
    {
        public Level Level { get; set; }
        
        public ProcessState ProcessState { get; set; }
        
        public GameSettings GameSettings { get; set; }
    }
}