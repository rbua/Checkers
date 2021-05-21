using CheckersGame.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CheckersGame
{
    static class GameManager
    {
        static Cell CurrentCell = new Cell();
        static Cell StartCell;

        public static Game Game;

        static List<Cell> SelectedCells;
        static List<Cell> FinishGameableCells;
        static List<Cell> FinishBeatableCells;

        static string ResourcePath;

        const string WhiteMarkerName = "WhiteMarker.png";
        const string BlackMarkerName = "BlackMarker.png";
        const string WhiteMarkerQueenName = "WhiteMarkerQueen.png";
        const string BlackMarkerQueenName = "BlackMarkerQueen.png";

        public static void InitializeGame(MainWindow window, string player1, string player2)
        {
            ResourcePath = GetResourcesDir();
            Game = Game.GetInstance(window, player1, player2);            
        }

        public static void Cell_Click(object sender, RoutedEventArgs e)
        {
            Cell cell = sender as Cell;

            if (IsCellValidForAction(cell))
            {
                MoveMark(cell);

                if (Game.GameOver)
                {
                    Game.SwapActivePlayer();
                    MessageBox.Show($"Game over. {Game.ActivePlayer.Name} won!");
                }
            }
            else if (cell.Owner != null && IfActivePlayerOwnsMarker(cell))
            {
                if (PlayerCanBeat())
                {
                    List<Cell> beatableCellsWithScenarios = GetBeatableCellsWithScenarios();
                    List<Cell> beatableCellsWithLongestScenarios = GetBeatableCellsWithLongestScenarios(beatableCellsWithScenarios);
                    List<Cell> finishBeatableCells = GetFinishBeatableCells(cell);

                    if (finishBeatableCells.Count > 0 && beatableCellsWithLongestScenarios.Contains(cell))
                    {
                        StartCell = cell;
                        FinishBeatableCells = finishBeatableCells;
                        finishBeatableCells.Add(cell);
                        SelectPotentialCells(finishBeatableCells);
                    }
                }
                else
                {
                    List<Cell> finishGameableCells = GetFinishGameableCells(cell);
                    if (finishGameableCells.Count > 0)
                    {
                        StartCell = cell;
                        FinishGameableCells = new List<Cell>(finishGameableCells);
                        finishGameableCells.Add(cell);
                        SelectPotentialCells(finishGameableCells);
                    }
                }
            }
        }

        public static void ResetScore()
        {
            if (Game != null)
            {
                Game.Player1.Score = Game.Player2.Score = 0;
                UpdateScore();
            }
        }

        public static bool ExistStartGameableCells()
        {
            bool result = false;

            foreach (var cell in GetAllCellsWithOwner())
            {
                if (GetFinishGameableCells(cell).Count > 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static IEnumerable<Cell> GetAllCellsWithOwner()
        {
            var result = GetAllCells().Where(cell => cell.Owner?.Color == Game.ActivePlayer.Color);
            return result;
        }

        public static IEnumerable<Cell> GetAllCells()
        {
            var result = Game.Window.ChessGrid.Children.OfType<Border>()
                .Select(border => border.Child as Cell);
            return result;
        }

        private static bool IfBestScenariosContainsThisCell(Cell cell)
        {
            bool result = false;

            foreach (var scenario in StartCell.BestBeatScenarios)
            {
                if (scenario.List.Contains(cell))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        private static List<Cell> GetBeatableCellsWithLongestScenarios(List<Cell> cells)
        {
            var result = cells.GroupBy
                        (cell => cell.MaxDepth,
                            (key, group) => new
                            {
                                Depth = key,
                                Cells = group
                            })
              .OrderByDescending(c => c.Depth)
              .FirstOrDefault()
              .Cells
              .ToList();

            return result;
        }

        public static bool PlayerCanBeat()
        {
            return Game.ActivePlayer?.AllStartBeatableCells?.Count > 0;
        }

        public static List<Cell> GetAllStartBeatableCells()
        {
            List<Cell> result = new List<Cell>();

            var cells = GetAllCellsWithOwner();

            foreach (var cell in cells)
            {
                if (GetFinishBeatableCells(cell).Count > 0)
                {
                    result.Add(cell);
                }
            }

            return result;
        }

        private static List<Cell> GetBeatableCellsWithScenarios()
        {
            List<Cell> startBeatableCells = GetAllStartBeatableCells();
            startBeatableCells.ForEach(delegate (Cell cell) { cell.BeatScenarios.Clear(); });

            foreach (var startBeatableCell in startBeatableCells)
            {
                CalculateScenarios(startBeatableCell);
            }

            return startBeatableCells;
        }

        private static void CalculateScenarios(Cell startBeatableCell, Scenario scenario = null, List<Cell> finishBeatableCells = null)
        {
            if (finishBeatableCells == null)
            {
                finishBeatableCells = GetFinishBeatableCells(startBeatableCell);
            }

            foreach (var cell in finishBeatableCells)
            {
                if (scenario == null)
                {
                    scenario = new Scenario();
                    startBeatableCell.BeatScenarios.Add(scenario);
                }
                scenario.List.Add(cell);
                CalculateScenarios(startBeatableCell, scenario, GetFinishBeatableCells(cell).Except(scenario.List).ToList());
            }
        }

        private static List<Cell> GetFinishBeatableCells(Cell startBeatableCell)
        {
            List<Position> FinishBeatablePositions = GetFinishPositions(startBeatableCell, true);
            return GetFinishCells(FinishBeatablePositions);
        }

        private static List<Cell> GetFinishGameableCells(Cell cell)
        {
            List<Position> finishGameablePositions = GetFinishPositions(cell);
            return GetFinishCells(finishGameablePositions);
        }

        private static bool IsCellValidForAction(Cell finishCell)
        {
            return StartCell != null && StartCell != finishCell && !IfCellHasMarker(finishCell);
        }

        private static void MoveMark(Cell finishCell)
        {
            bool beat = FinishBeatableCells?.Count > 0;
            List<Cell> cells = beat && IfBestScenariosContainsThisCell(finishCell) ? FinishBeatableCells : FinishGameableCells;
            bool isCellExistInList = cells.Where(c => c.CellPosition == finishCell.CellPosition).Count() > 0;

            if (isCellExistInList)
            {
                UnAssignCells(SelectedCells);
                finishCell.Owner = StartCell.Owner.Copy<Marker>();
                StartCell.Owner.Color = MarkerColor.Undefined;
                TurnOwnerToQueen(finishCell);
                finishCell.AddImageMarker(finishCell.Owner.Color, finishCell.Owner.IsQueen);
                StartCell.RemoveImageMarker();
                SelectedCells?.Clear();
                FinishGameableCells?.Clear();
                FinishBeatableCells?.Clear();

                if (beat)
                {
                    Game.ActivePlayer.Score++;
                    UpdateScore();
                    RemoveDeadMark(finishCell);

                    if (GetFinishBeatableCells(finishCell).Count == 0)
                    {
                        Game.SwapActivePlayer();
                    }
                }
                else
                {
                    Game.SwapActivePlayer();
                }

                StartCell = null;
            }
        }

        public static void UpdateScore()
        {
            Game.Window.txtBlockScore.Text = $"{Game.Player1.Score.ToString()} : {Game.Player2.Score.ToString()}";
        }

        private static void TurnOwnerToQueen(Cell finishCell)
        {
            if (!finishCell.Owner.IsQueen)
            {
                int targetRow = Game.ActivePlayer.Color == MarkerColor.Black ? 7 : 0;
                finishCell.Owner.IsQueen = finishCell.CellPosition.X == targetRow;
            }
        }

        private static void RemoveDeadMark(Cell finishCell)
        {
            int x = (StartCell.CellPosition.X + finishCell.CellPosition.X) / 2;
            int y = (StartCell.CellPosition.Y + finishCell.CellPosition.Y) / 2;
            Position position = new Position(x, y);

            Cell deadMark = Game.Window.ChessGrid.Children.OfType<Border>()
                .Select(border => border.Child as Cell)
                .Where(cell => cell.CellPosition != null && cell.CellPosition.Equals(position)).FirstOrDefault();

            if (deadMark != null && deadMark.Owner != null)
            {
                deadMark.Owner.Color = MarkerColor.Undefined;
            }

            deadMark.RemoveImageMarker();
        }

        private static void AssignCell(Cell cell, int thickness = 2)
        {
            if (cell.CellBorder != null)
            {
                cell.CellBorder.BorderBrush = Brushes.Green;
                cell.CellBorder.BorderThickness = new Thickness(thickness);
            }
        }

        private static void UnAssignCell(Cell cell)
        {
            AssignCell(cell, 0);
        }

        private static void SelectPotentialCells(List<Cell> cells)
        {
            if (cells != null)
            {
                if (SelectedCells != null)
                {
                    UnAssignCells(SelectedCells);
                }

                SelectedCells = new List<Cell>(cells);
                cells.ForEach(cell => AssignCell(cell));
            }
        }

        private static void UnAssignCells(List<Cell> cells)
        {
            if (cells != null)
            {
                cells.ForEach(cell => UnAssignCell(cell));
                cells.Clear();
            }
        }

        private static bool IfActivePlayerOwnsMarker(Cell cell)
        {
            return cell != null && cell.Owner.Color == Game.ActivePlayer.Color;
        }

        private static bool IfCellHasMarker(Cell cell)
        {
            return cell?.Owner?.Color != MarkerColor.Undefined;
        }

        private static List<Position> GetFinishPositions(Cell currentCell, bool beatable = false)
        {
            List<Position> positions = new List<Position>();

            int j = beatable ? 2 : 1;
            int vector = currentCell.Owner.Color == MarkerColor.Black ? j : -j;
            int x = currentCell.CellPosition.X;
            int y = currentCell.CellPosition.Y;
            int i = y - j;
            i = i < 0 ? 0 : i;

            while (i < 8 && i <= y + j)
            {
                if (i != y)
                {
                    if (!beatable)
                    {
                        positions.Add(new Position(x + vector, i));

                        if (currentCell.Owner.IsQueen)
                        {
                            positions.Add(new Position(x - vector, i));
                        }
                    }
                    else
                    {
                        for (int f = 0; f < 2; f++)
                        {
                            if (i != y - 1 && i != y + 1 && IsThereEnemyMark(new Position((2 * x + vector) / 2, (y + i) / 2)))
                            {
                                positions.Add(new Position(x + vector, i));
                            }
                            vector = -vector;
                        }
                    }
                }
                i++;
            }

            return positions;
        }

        private static bool IsThereEnemyMark(Position position)
        {
            MarkerColor enemyColor = GetEnemyColor();
            Cell cell = GetCell(position);
            return cell != null && cell.Owner.Color == enemyColor;
        }

        public static MarkerColor GetEnemyColor()
        {
            return Game.ActivePlayer.Color == MarkerColor.Black ? MarkerColor.White : MarkerColor.Black;
        }

        private static Cell GetCell(Position position)
        {
            Cell result = Game.Window.ChessGrid.Children.OfType<Border>()
                .Select(border => border.Child as Cell)
                .Where(cell => cell.CellPosition != null && cell.CellPosition.Equals(position)).FirstOrDefault();
            return result;
        }

        private static List<Cell> GetFinishCells(List<Position> positions)
        {
            List<Cell> result = Game.Window.ChessGrid.Children.OfType<Border>()
                                 .Select(b => b.Child as Cell)
                                 .Where(cell => positions.Contains(cell.CellPosition)
                                 && (cell.Owner?.Color == MarkerColor.Undefined)).ToList();
            return result;
        }

        public static bool IsDisabledCell(int i)
        {
            return (i + i / 8) % 2 == 0;
        }

        private static string GetResourcesDir()
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int i = currentDir.LastIndexOf("bin");
            string result = currentDir.Remove(i, currentDir.Length - i);            
            return result + "Resources\\";
        }

        public static string GetMarkerPath(MarkerColor color, bool isQueen)
        {          
            string markerName = isQueen ?
                (color == MarkerColor.White ? WhiteMarkerQueenName : BlackMarkerQueenName) :
                (color == MarkerColor.White ? WhiteMarkerName : BlackMarkerName);
            return ResourcePath + markerName;
        }
    }
}
