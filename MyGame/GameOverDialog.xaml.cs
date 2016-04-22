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

namespace MyGame
{
    /// <summary>
    /// Interaction logic for GameOverDialog.xaml
    /// </summary>
    public partial class GameOverDialog : Window
    {
        private MainWindow MainWindow;
        public GameOverDialog(MainWindow window, bool won = false)
        {
            InitializeComponent();

            MainWindow = window;

            if (won)
            {
                label.Content = "Level Completed";
            }
            else
            {
                label.Content = "Game Over";
            }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Reset();
            Close();
        }
    }
}
