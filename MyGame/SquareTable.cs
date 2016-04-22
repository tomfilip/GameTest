using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyGame
{
    public class SquareTable: GameTable
    {
        public readonly Size BaseSquareSize;
        public readonly Size SizeInBlocks;



        public SquareTable(Size baseSquareSize, Point position, Size sizeInBlocks, UIElement uiContainer)
            :base(position, new Size(baseSquareSize.Width * sizeInBlocks.Width, baseSquareSize.Height * sizeInBlocks.Height),uiContainer)
        {
            BaseSquareSize = baseSquareSize;
            SizeInBlocks = sizeInBlocks;
        }
    }
}
