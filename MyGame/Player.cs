using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MyGame
{
    public class Player: TableItem<SquareTable>
    {
        public Direction Direction { get; set; }
        public Point Coordinates{get; protected set;}
        public PlayerControls Controls{get;private set;}

        public Player(PlayerControls controls, UIElement uiElement, int xCoord, int yCoord, SquareTable squareTable)
            :base(new Point(squareTable.BaseSquareSize.Width * xCoord, squareTable.BaseSquareSize.Height * yCoord), squareTable.BaseSquareSize, uiElement, squareTable)
        {
            if (uiElement != null)
            {
                uiElement.RenderTransformOrigin = new Point(0.5, 0.5);
            }
            
            Direction = Direction.Down;
            Controls = controls;
            Coordinates = new Point(xCoord, yCoord);
        }

        internal override void Destroy()
        {
            base.Destroy();
            if (this.GetType() == typeof(Player))
            {
                GameTable.OnGameOver();
            }
            
        }

        public void KeyEventHandle(KeyEventArgs e)
        {
            if (e.Key == Controls.Up){
                if (Coordinates.Y > 0)
                {
                    Coordinates = new Point(Coordinates.X, Coordinates.Y - 1);
                    Direction = Direction.Up;
                    UiContainer.RenderTransform = new RotateTransform(90);
                }
            }
            else if(e.Key == Controls.Down)
            {
                if (Coordinates.Y < GameTable.SizeInBlocks.Height - 1)
                {
                    Coordinates = new Point(Coordinates.X, Coordinates.Y + 1);
                    Direction = Direction.Down;
                    UiContainer.RenderTransform = new RotateTransform(-90);
                }
            }
            else if (e.Key == Controls.Left)
            {
                if (Coordinates.X > 0)
                {
                    Coordinates = new Point(Coordinates.X - 1, Coordinates.Y);
                    Direction = Direction.Left;
                    UiContainer.RenderTransform = new RotateTransform(0);
                }
            }
            else if (e.Key == Controls.Right)
            {
                if (Coordinates.X < GameTable.SizeInBlocks.Width - 1)
                {
                    Coordinates = new Point(Coordinates.X + 1, Coordinates.Y);
                    Direction = Direction.Right;
                    UiContainer.RenderTransform = new RotateTransform(180);
                }
            }
            else if (e.Key == Controls.Shoot)
            {
                int bulletXCoord = 0;
                int bulletYCoord = 0;
                if (Direction == Direction.Left)
                {
                    bulletXCoord = (int)(this.Coordinates.X - 1);
                    bulletYCoord = (int)(this.Coordinates.Y);
                }
                else if (Direction == Direction.Right)
                {
                    bulletXCoord = (int)(this.Coordinates.X + 1);
                    bulletYCoord = (int)(this.Coordinates.Y);
                }
                else if (Direction == Direction.Up)
                {
                    bulletXCoord = (int)(this.Coordinates.X);
                    bulletYCoord = (int)(this.Coordinates.Y - 1);
                }
                else if (Direction == Direction.Down)
                {
                    bulletXCoord = (int)(this.Coordinates.X);
                    bulletYCoord = (int)(this.Coordinates.Y + 1);
                }
                Bullet<Ghost, Player> bullet = new Bullet<Ghost, Player>(this, bulletXCoord, bulletYCoord, GameTable, Direction);
            }
        }

        protected override void ItemsDrawing(object sender, DrawEventArgs e)
        {

            Position = new Point(Coordinates.X * GameTable.BaseSquareSize.Width, Coordinates.Y * GameTable.BaseSquareSize.Height);
            base.ItemsDrawing(sender, e);
        }

        protected void AdjustCoordinatesToFitIntoTable()
        {
            if (this.Coordinates.X < 0)
            {
                this.Coordinates = new Point(0, this.Coordinates.Y);
            }
            else if (this.Coordinates.X >= GameTable.SizeInBlocks.Width)
            {
                this.Coordinates = new Point(GameTable.SizeInBlocks.Width - 1, this.Coordinates.Y);
            }

            if (this.Coordinates.Y < 0)
            {
                this.Coordinates = new Point(this.Coordinates.X, 0);
            }
            else if (this.Coordinates.Y >= GameTable.SizeInBlocks.Height)
            {
                this.Coordinates = new Point(this.Coordinates.X, GameTable.SizeInBlocks.Height - 1);
            }
        }
    }

    public class PlayerControls
    {
        public Key Up{get; set;}
        public Key Down{get;set;}
        public Key Left{get;set;}
        public Key Right{get;set;}
        public Key Shoot { get; set; }
    }
}
