using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyGame
{
    public class SquareTable: GameTable
    {
        public readonly Size BaseSquareSize;
        public readonly Size SizeInBlocks;

        public delegate void GameOverAction();
        public event GameOverAction GameOver;

        public delegate void LevelWonAction();
        public event LevelWonAction LevelWon;

        public SquareTable(Size baseSquareSize, Point position, Size sizeInBlocks, Canvas uiContainer)
            :base(position, new Size(baseSquareSize.Width * sizeInBlocks.Width, baseSquareSize.Height * sizeInBlocks.Height), uiContainer)
        {
            BaseSquareSize = baseSquareSize;
            SizeInBlocks = sizeInBlocks;

            GameOver += SquareTable_GameOver;
            LevelWon += SquareTable_LevelWon;
        }

        private void SquareTable_LevelWon()
        {

        }

        private void SquareTable_GameOver()
        {
            base.Stop();
            base.ClearItems();
        }

        internal virtual void OnGameOver()
        {
            if (GameOver != null)
            {
                GameOver();
            }
        }

        internal virtual void OnLevelWon()
        {
            if (LevelWon != null)
            {
                LevelWon();
            }
        }
    }
}
