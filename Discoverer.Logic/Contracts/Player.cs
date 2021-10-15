using System;

namespace Discoverer.Logic.Contracts
{
    public class Player
    {
        public int Order { get; set; }
        
        public string Name { get; set; }
        
        public Guid Id { get; set; }
    }
}