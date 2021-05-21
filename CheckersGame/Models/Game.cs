using CheckersGame.JsonModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CheckersGame.Models
{
    public class Game
    {
        private static Game _instance;
        private Player _activePlayer;

        public Board Board { get; private set; }
        public bool GameOver
        {
            get
            {
                return IsGameOver();
            }
        }
        public MainWindow Window { get; private set; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public Player ActivePlayer
        {
            get
            {
                return _activePlayer;
            }
            private set
            {
                SetActivePlayerBorder(value);
                _activePlayer = value;
            }
        }

        protected Game(MainWindow window, string player1Name, string player2Name)
        {
            Window = window;
            Player1 = new Player(MarkerColor.Black, player1Name, true);
            Player2 = new Player(MarkerColor.White, player2Name, false);
            InitializeNewGame();
        }

        private void InitializeNewGame()
        {
            Board = new Board(Window);
            ActivePlayer = Player1;
            InitializePlayers();
            AddEventHandlersToCells();
            GameManager.ResetScore();
        }

        public void LoadNewGame(GameModel model)
        {
            ClearBordersAndCells();
            Player1 = model.Player1;
            Player2 = model.Player2;
            ActivePlayer = model.Player1 != null && model.Player1.IsActive ? Player1 : Player2;
            Board = new Board(Window, model.Cells);
            InitializePlayers();
            AddEventHandlersToCells();
            GameManager.UpdateScore();
        }

        public void RestartGame()
        {
            ClearBordersAndCells();
            InitializeNewGame();
        }

        private void ClearBordersAndCells()
        {
            int intTotalChildren = Window.ChessGrid.Children.Count - 1;

            for (int intCounter = intTotalChildren; intCounter > 0; intCounter--)
            {
                if (Window.ChessGrid.Children[intCounter].GetType() == typeof(Border))
                {
                    Border currentChild = (Border)Window.ChessGrid.Children[intCounter];
                    Window.ChessGrid.Children.Remove(currentChild);
                }
            }
        }

        public static Game GetInstance(MainWindow window, string player1Name, string player2Name)
        {
            if (_instance == null)
            {
                _instance = new Game(window, player1Name, player2Name);
            }

            return _instance;
        }

        public void SetActivePlayerProps(int score, bool isActive = true)
        {
            if (Player1.IsActive)
            {
                Player1.Score = score;
                Player1.IsActive = isActive;
                Player2.IsActive = !isActive;
            }
            else
            {
                Player2.Score = score;
                Player2.IsActive = isActive;
                Player1.IsActive = !isActive;
            }
        }

        public void SwapActivePlayer()
        {
            ActivePlayer.AllStartBeatableCells = null;

            if (ActivePlayer == Player1)
            {
                Player1.IsActive = false;
                Player2.IsActive = true;
                ActivePlayer = Player2;
            }
            else
            {
                Player1.IsActive = true;
                Player2.IsActive = false;
                ActivePlayer = Player1;
            }
        }

        private bool IsGameOver()
        {
            MarkerColor color = ActivePlayer.Color;
            bool isAnyEnemyMark = GameManager.GetAllCellsWithOwner().Any();
            bool existStartBeatableCells = GameManager.PlayerCanBeat();
            bool existStartGameableCells = GameManager.ExistStartGameableCells();

            return !isAnyEnemyMark || (!existStartBeatableCells && !existStartGameableCells);
        }

        private void SetActivePlayerBorder(Player player)
        {
            if (player == Player1)
            {
                Board.ActivePlayerBorder = Window.Player1LabelBorder;
                Board.InActivePlayerBorder = Window.Player2LabelBorder;
            }
            else
            {
                Board.ActivePlayerBorder = Window.Player2LabelBorder;
                Board.InActivePlayerBorder = Window.Player1LabelBorder;
            }

            AssignActivePlayerLabel();
        }

        private void AssignActivePlayerLabel()
        {
            Board.ActivePlayerBorder.BorderThickness = new Thickness(2);
            Board.InActivePlayerBorder.BorderThickness = new Thickness(0);
        }

        private void AddEventHandlersToCells()
        {
            foreach (var child in Window.ChessGrid.Children)
            {
                var cell = (child as Border)?.Child;

                if (!(cell as Cell)?.Disabled ?? false)
                {
                    ((Cell)cell).MouseLeftButtonDown += GameManager.Cell_Click;
                }
            }
        }

        private void InitializePlayers()
        {
            Window.txtBlockPlayer1Name.Text = "Player 1: " + Player1.Name;
            Window.txtBlockPlayer2Name.Text = "Player 2: " + Player2.Name;
        }
    }
}
