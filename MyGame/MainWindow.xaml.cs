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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player Player { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            SquareTable table = new SquareTable(new Size(30, 30), new Point(0, 0), new Size(20, 20), Container);
            table.Start();
            Player = new Player(new PlayerControls() { Up = Key.Up, Down = Key.Down, Left = Key.Left, Right = Key.Right }, rect, 0, 0, table);


        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Player.KeyEventHandle(e);
        }
    }
}
