using System.Collections.Generic;

namespace CheckersGame.Models
{
    public class Scenario
    {
        public List<Cell> List { get; set; }
            
        public Scenario()
        {
            List = new List<Cell>();         
        }      
    }
}
