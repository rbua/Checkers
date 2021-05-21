using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using CheckersGame.Models;

namespace CheckersGame.JsonModels
{
    [Serializable]
    public class GameModel
    {
        public DateTime DateCreated { get; set; }
        public int Id { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public List<CellModel> Cells { get; set; }

        public static List<CellModel> GetCurrentCells()
        {
            var cells = GameManager.GetAllCells();
            List<CellModel> result = new List<CellModel>();

            foreach (var cell in cells)
            {
                result.Add(new CellModel(cell.CellPosition, cell.Owner, cell.Disabled));
            }

            return result;
        }
    }
}
