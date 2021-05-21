using CheckersGame.JsonModels;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CheckersGame.Models
{
    public class Board
    {
        MainWindow Window;
        public Border ActivePlayerBorder { get; set; }
        public Border InActivePlayerBorder { get; set; }

        public Board(MainWindow window)
        {
            Window = window;
            DrawBoardAndMarkers();
        }

        public Board(MainWindow window, List<CellModel> cellModels)
        {
            Window = window;
            ReDrawBoardAndMarkers(cellModels);
        }

        private void SetupCell(Position position, bool isDisabled, Marker owner, bool isLoaded)
        {
            SolidColorBrush defaultBrush = new SolidColorBrush(Colors.Gainsboro);
            SolidColorBrush alternateBrush = new SolidColorBrush(Color.FromRgb(84, 65, 39));

            Border border = new Border();
            Cell cell = new Cell();
            cell.Owner = owner;
            cell.CellPosition = position;
            border.Child = cell;
            Window.ChessGrid.Children.Add(border);

            if (isDisabled)
            {
                cell.Disabled = true;
                cell.Background = defaultBrush;
            }
            else
            {
                if (!isLoaded)
                {
                    cell.Owner = new Marker();
                }
                cell.Background = alternateBrush;
                GenerateMarker(cell, isLoaded ? -1 : position.X);
                cell.Cursor = Cursors.Hand;
                cell.CellBorder = border;
            }

            Grid.SetRow(border, cell.CellPosition.X);
            Grid.SetColumn(border, cell.CellPosition.Y);
        }
        private void ReDrawBoardAndMarkers(List<CellModel> cellModels)     
        {         
            foreach (var cellModel in cellModels)
            {                
                 SetupCell(cellModel.CellPosition, cellModel.Disabled, cellModel.Owner, true);              
            }
        }

        private void DrawBoardAndMarkers()
        {
            int counter = 0;  
                 
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Position position = new Position(i, j);
                    SetupCell(position, GameManager.IsDisabledCell(counter), new Marker(), false);                
                    counter++;
                }
            }
        }      
       
        private void GenerateMarker(Cell cell, int rowNumber)
        {
            if (rowNumber >= 0)
            {
                MarkerColor color = rowNumber < 3 ? MarkerColor.Black :
                    rowNumber > 4 ? MarkerColor.White : MarkerColor.Undefined;
                cell.AddImageMarker(color);
                cell.Owner.Color = color;
            }
            else if (cell?.Owner?.Color != MarkerColor.Undefined)
            {
                cell.AddImageMarker(cell.Owner.Color, cell.Owner.IsQueen);
            }
        }
    }
}
