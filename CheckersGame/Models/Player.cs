using Newtonsoft.Json;
using System.Collections.Generic;

namespace CheckersGame.Models
{
    public class Player
    {
        private List<Cell> _allStartBeatableCells;

        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsActive { get; set; }
        public MarkerColor Color { get; set; }
        [JsonIgnore]
        public List<Cell> AllStartBeatableCells
        {
            get
            {
                return _allStartBeatableCells ?? (_allStartBeatableCells = GameManager.GetAllStartBeatableCells());
            }
            set
            {
                _allStartBeatableCells = value;
            }
        }

        public Player(MarkerColor color, string name, bool isActive, int score = 0)
        {
            Name = name;
            IsActive = isActive;
            Score = score;
            Color = color;
        }
    }
}
