using CheckersGame.Models;
using CheckersGame.JsonModels;
using System.Windows;
using System.Linq;
using System;
using System.Dynamic;
using Newtonsoft.Json;

namespace CheckersGame
{
    public class GameModelStoreModel
    {
        public int Id { get; set; }
        public string GameModelModel { get; set; }
        public DateTime DateCreated { get; set; }
    }


    public partial class MainWindow : Window
    {
        public MainWindow(string player1, string player2)
        {
            InitializeComponent();
            GameManager.InitializeGame(this, player1, player2);
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Game game = GameManager.Game;

            if (!game.GameOver)
            {
                GameModel model = new GameModel
                {
                    Player2 = game.Player2,
                    Player1 = game.Player1,
                    Cells = GameModel.GetCurrentCells(),
                    DateCreated = DateTime.Now
                };

                var storeModel = new GameModelStoreModel
                {
                    GameModelModel = JsonConvert.SerializeObject(model, Formatting.Indented),
                    DateCreated = DateTime.Now
                };

                var context = new GameModelContext(@"Data Source=.\SQLEXPRESS;Initial Catalog=Players;Integrated Security=True");
                context.GameModels.Add(storeModel);
                context.SaveChanges();

                MessageBox.Show("Successfully saved");            
            }       
        }

        private void LoadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GameModel model = new GameModel();
            try
            {
                var context = new GameModelContext(@"Data Source=.\SQLEXPRESS;Initial Catalog=Players;Integrated Security=True");
                var storeModel = context.GameModels.OrderByDescending(x => x.DateCreated).FirstOrDefault();
                model = JsonConvert.DeserializeObject<GameModel>(storeModel.GameModelModel);

                GameManager.Game.LoadNewGame(model);
            }
            catch
            {
                MessageBox.Show("Erorr while loading data");
            }
        }

        private void RestartItem_Click(object sender, RoutedEventArgs e)
        {
            GameManager.Game.RestartGame();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
