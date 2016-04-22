using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyGame
{
    public class Player: TableItem<SquareTable>
    {
        public Point Coordinates{get;private set;}
        public PlayerControls Controls{get;private set;}

        public Player(PlayerControls controls, UIElement uiElement, int xCoord, int yCoord, SquareTable squareTable)
            :base(new Point(squareTable.BaseSquareSize.Width * xCoord, squareTable.BaseSquareSize.Height * yCoord), squareTable.BaseSquareSize, uiElement, squareTable)
        {
            Controls = controls;
            Coordinates = new Point(xCoord, yCoord);
        }

        public void KeyEventHandle(KeyEventArgs e)
        {
            if (e.Key == Controls.Up){
                Coordinates = new Point(Coordinates.X, Coordinates.Y -1);
                Position = new Point(Position.X, Coordinates.Y * GameTable.BaseSquareSize.Height);
            }
            else if(e.Key == Controls.Down)
            {
                Coordinates = new Point(Coordinates.X, Coordinates.Y + 1);
                Position = new Point(Position.X, Coordinates.Y * GameTable.BaseSquareSize.Height);
            }
            else if (e.Key == Controls.Left)
            {
                Coordinates = new Point(Coordinates.X - 1, Coordinates.Y);
                Position = new Point(Coordinates.X * GameTable.BaseSquareSize.Width, Position.Y);
            }
            else if (e.Key == Controls.Right)
            {
                Coordinates = new Point(Coordinates.X + 1, Coordinates.Y);
                Position = new Point(Coordinates.X * GameTable.BaseSquareSize.Width, Position.Y);
            }
        }
    }

    public class PlayerControls
    {
        public Key Up{get; set;}
        public Key Down{get;set;}
        public Key Left{get;set;}
        public Key Right{get;set;}
    }
}
