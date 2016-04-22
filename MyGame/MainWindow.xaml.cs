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
using System.Windows.Threading;

namespace MyGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player Player { get; set; }
        SquareTable Table { get; set; }
        DispatcherTimer Timer { get; set; }
        DateTime TimeStart { get; set; }
        DateTime TimeEnd { get; set; }

        public MainWindow()
        {
            

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 10000);
            Timer.Tick += Timer_Tick;

            InitializeComponent();
            Table = new SquareTable(new Size(30, 30), new Point(0, 0), new Size(60, 30), Container);

            Reset();

            Table.GameOver += Table_GameOver;
            Table.LevelWon += Table_LevelWon;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var random = new Random();
            var randX = random.Next(0, (int)((Table.SizeInBlocks.Width - 1)));
            var randY = random.Next(0, (int)((Table.SizeInBlocks.Height - 1)));


            Rectangle ghostUi = new Rectangle();
            ghostUi.Fill = new ImageBrush(new BitmapImage(new Uri(@"H:\Programovanie\2016\GameTest\MyGame\Images\SoldierSprite.png")));
            ghostUi.VerticalAlignment = VerticalAlignment.Top;
            ghostUi.HorizontalAlignment = HorizontalAlignment.Left;
            ghostUi.Width = Table.BaseSquareSize.Width;
            ghostUi.Height = Table.BaseSquareSize.Height;
            Canvas.SetLeft(Container, Table.BaseSquareSize.Width * randX);
            Canvas.SetTop(Container, Table.BaseSquareSize.Height * randY);
            Container.Children.Add(ghostUi);

            Ghost ghostPlayer = new Ghost(Player, randX, randY, ghostUi, Table);
        }

        private void Table_LevelWon()
        {
            GameOverDialog gameoverDialog = new GameOverDialog(this, true);
            gameoverDialog.ShowDialog();
        }

        private void Table_GameOver()
        {
            Container.Children.Clear();
            lock (Table._Items)
            {
                var listCopy = Table._Items.ToList();
                foreach (var item in listCopy)
                {
                    item.Value.Destroy();
                }
            }
            Table.ClearItems();

            GameOverDialog gameoverDialog = new GameOverDialog(this);
            gameoverDialog.ShowDialog();
        }

        public void Reset()
        {
            

            //if (Player != null)
            //{
            //    Player.Destroy();
            //}
            
            Table.Start();
            Rectangle playerUi = new Rectangle();
            playerUi.Fill = new ImageBrush(new BitmapImage(new Uri(@"H:\Programovanie\2016\GameTest\MyGame\Images\PlayerSprite.jpg")));
            playerUi.VerticalAlignment = VerticalAlignment.Top;
            playerUi.HorizontalAlignment = HorizontalAlignment.Left;
            playerUi.Height = Table.BaseSquareSize.Height;
            playerUi.Width = Table.BaseSquareSize.Height;

            Container.Children.Add(playerUi);

            Rectangle ghostUi = new Rectangle();
            ghostUi.Fill = new ImageBrush(new BitmapImage(new Uri(@"H:\Programovanie\2016\GameTest\MyGame\Images\SoldierSprite.png")));
            ghostUi.VerticalAlignment = VerticalAlignment.Top;
            ghostUi.HorizontalAlignment = HorizontalAlignment.Left;
            ghostUi.Width = Table.BaseSquareSize.Width;
            ghostUi.Height = Table.BaseSquareSize.Height;
            Canvas.SetLeft(Container, Table.BaseSquareSize.Width * (Table.SizeInBlocks.Width - 1));
            Canvas.SetTop(Container, Table.BaseSquareSize.Height * (Table.SizeInBlocks.Height - 1));
            Container.Children.Add(ghostUi);

            Player = new Player(new PlayerControls() { Up = Key.Up, Down = Key.Down, Left = Key.Left, Right = Key.Right, Shoot = Key.Space }, playerUi, 0, 0, Table);
            Ghost ghostPlayer = new Ghost(Player, (int)(Table.SizeInBlocks.Width - 1), (int)(Table.SizeInBlocks.Height - 1), ghostUi, Table);

            //Timer.Start();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Player.KeyEventHandle(e);
        }
    }
}
