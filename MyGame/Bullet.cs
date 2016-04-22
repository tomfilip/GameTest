using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyGame
{
    public class Bullet<TTarget, TOwner> : Player
        where TTarget: Player
        where TOwner : Player
    {
        private DispatcherTimer Timer { get; set; }
        public Direction Direction { get; set; }
        private Rectangle bulletUi { get; set; }

        public Bullet(TOwner owner, int xCoord, int yCoord, SquareTable table, Direction direction)
            : base(null, null, xCoord, yCoord, table)
        {
            Direction = direction;
            bulletUi = new Rectangle();
            bulletUi.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            bulletUi.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            if (typeof(TTarget) == typeof(Ghost))
            {
                bulletUi.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else
            {
                bulletUi.Fill = new SolidColorBrush(Color.FromRgb(115, 220, 24));
            }
            

            Canvas.SetLeft(GameTable.UiContainer, GameTable.BaseSquareSize.Width * xCoord);
            Canvas.SetTop(GameTable.UiContainer, GameTable.BaseSquareSize.Height * yCoord);

            table.UiContainer.Children.Add(bulletUi);

            ChangeDirection(direction);
            base.UiContainer = bulletUi;





            Timer = new DispatcherTimer();

            Timer.Interval = new TimeSpan(0, 0, 0, 0, 700);
            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SeachPlayerCollision();
            var newInterval = (int)(Timer.Interval.Milliseconds * 0.80);
            if (newInterval > 50)
            {
                Timer.Interval = new TimeSpan(0, 0, 0, 0, newInterval);
            }
            
            if (Direction == Direction.Down)
            {
                if (this.Coordinates.Y + 1 >= GameTable.SizeInBlocks.Height)
                {
                    Destroy();
                }
                else
                {
                    this.Coordinates = new System.Windows.Point(this.Coordinates.X, this.Coordinates.Y + 1);
                }
                
            }
            else if (Direction == Direction.Left)
            {
                if (this.Coordinates.X - 1 < 0)
                {
                    Destroy();
                }
                else
                {
                    this.Coordinates = new System.Windows.Point(this.Coordinates.X - 1, this.Coordinates.Y);
                }
            }
            else if (Direction == Direction.Right)
            {
                if (this.Coordinates.X + 1 >= GameTable.SizeInBlocks.Width)
                {
                    Destroy();
                }
                else
                {
                    this.Coordinates = new System.Windows.Point(this.Coordinates.X + 1, this.Coordinates.Y);
                }
            }
            else if (Direction == Direction.Up)
            {
                if (this.Coordinates.Y - 1 < 0)
                {
                    Destroy();
                }
                else
                {
                    this.Coordinates = new System.Windows.Point(this.Coordinates.X, this.Coordinates.Y - 1);
                }
            }
            SeachPlayerCollision();
        }

        internal override void Destroy()
        {
            Timer.Stop();
            GameTable.RemoveItem(this);
        }


        private void SeachPlayerCollision()
        {
            lock (GameTable._Items)
            {
                foreach(var item in GameTable._Items)
                {
                    if (item.Value.GetType() == typeof(TTarget))
                    {
                        var target = (TTarget)item.Value;
                        if(this.Coordinates.X == target.Coordinates.X && this.Coordinates.Y == target.Coordinates.Y)
                        {

                            target.Destroy();
                            this.Destroy();
                            break;
                        }
                    }
                }
            }
        }

        public void ChangeDirection(Direction newDirection)
        {
            if (newDirection == Direction.Down || newDirection == Direction.Up)
            {
                bulletUi.Width = base.GameTable.BaseSquareSize.Width / 30;
                bulletUi.Height = base.GameTable.BaseSquareSize.Height;
                bulletUi.Margin = new System.Windows.Thickness(base.GameTable.BaseSquareSize.Width / 3, 0, 0, 0);
            }
            else
            {
                bulletUi.Width = base.GameTable.BaseSquareSize.Width;
                bulletUi.Height = base.GameTable.BaseSquareSize.Height / 30;
                bulletUi.Margin = new System.Windows.Thickness(0, base.GameTable.BaseSquareSize.Height / 3, 0, 0);
            }
            Direction = newDirection;
        }
    }
}
