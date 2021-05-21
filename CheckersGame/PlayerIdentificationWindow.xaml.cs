using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckersGame
{
    /// <summary>
    /// Interaction logic for PlayerIdentificationWindow.xaml
    /// </summary>
    public partial class PlayerIdentificationWindow : Window
    {
        public PlayerIdentificationWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {       
            MainWindow mainWindow = new MainWindow(this.textBox1.Text, this.textBox2.Text);
            mainWindow.Show();
            this.Close();
        }
    }
}
