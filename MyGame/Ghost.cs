using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MyGame
{
    public class Ghost : Player
    {
        public Player Player { get; private set; }
        public DispatcherTimer GhostTimer { get; private set; }
        public DispatcherTimer BulletDodgeReactionTimer { get; private set; }

        public Ghost(Player playerItem, int xCoord, int yCoord, UIElement uiElement, SquareTable table)
            :base(null, uiElement, xCoord, yCoord, table)
        {   
            GhostTimer = new DispatcherTimer();
            Player = playerItem;
            GhostTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            GhostTimer.Tick += Timer_Tick;
            GhostTimer.Start();

            BulletDodgeReactionTimer = new DispatcherTimer();
            BulletDodgeReactionTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            BulletDodgeReactionTimer.Tick += BulletDodgeReactionTimer_Tick;
            BulletDodgeReactionTimer.Start();
        }

        private void BulletDodgeReactionTimer_Tick(object sender, EventArgs e)
        {
            Bullet<Ghost, Player> BulletToDodge = null;
            if (IsBulletCollisionTrajectory(this.Coordinates, out BulletToDodge))
            {
                TryDodgeBullet(BulletToDodge);
            }
            AdjustCoordinatesToFitIntoTable();
        }

        private int TickCount = 0;

        private void Timer_Tick(object sender, EventArgs e)
        {

            GhostTimer.Interval = new TimeSpan(0, 0, 0, 0, new Random().Next(0, 2000));

            if (TickCount >= 3)
            {
                TickCount = 0;

                if (Direction == Direction.Left)
                {
                    Bullet<Player, Ghost> ghostBullet = new Bullet<Player, Ghost>(this, (int)(this.Coordinates.X - 1), (int)(this.Coordinates.Y), GameTable, this.Direction);
                }
                else if (Direction == Direction.Right)
                {
                    Bullet<Player, Ghost> ghostBullet = new Bullet<Player, Ghost>(this, (int)(this.Coordinates.X + 1), (int)(this.Coordinates.Y), GameTable, this.Direction);
                }
                else if (Direction == Direction.Up)
                {
                    Bullet<Player, Ghost> ghostBullet = new Bullet<Player, Ghost>(this, (int)(this.Coordinates.X), (int)(this.Coordinates.Y - 1), GameTable, this.Direction);
                }
                else if (Direction == Direction.Down)
                {
                    Bullet<Player, Ghost> ghostBullet = new Bullet<Player, Ghost>(this, (int)(this.Coordinates.X), (int)(this.Coordinates.Y + 1), GameTable, this.Direction);
                }

            }

            Bullet<Ghost, Player> BulletToDodge = null;
            

            //only if the ghost is not trying to dodge the bullet
            if (IsBulletCollisionTrajectory(this.Coordinates, out BulletToDodge) == false)
            {
                //Find the player and follow him
                if (Player.Coordinates.X > this.Coordinates.X)
                {
                    var newCoord = new Point(this.Coordinates.X + 1, this.Coordinates.Y);
                    if (!IsBulletCollisionTrajectory(newCoord))
                    {
                        this.Direction = Direction.Right;
                        UiContainer.RenderTransform = new RotateTransform(90);
                        this.Coordinates = newCoord;
                    }
                    
                }
                else if (Player.Coordinates.X < this.Coordinates.X)
                {
                    var newCoord = new Point(this.Coordinates.X - 1, this.Coordinates.Y);
                    if (!IsBulletCollisionTrajectory(newCoord))
                    {
                        this.Direction = Direction.Left;
                        UiContainer.RenderTransform = new RotateTransform(-90);
                        this.Coordinates = newCoord;
                    }
                }

                if (Player.Coordinates.Y > this.Coordinates.Y)
                {
                    var newCoord = new Point(this.Coordinates.X, this.Coordinates.Y + 1);
                    if (!IsBulletCollisionTrajectory(newCoord))
                    {
                        this.Direction = Direction.Down;
                        UiContainer.RenderTransform = new RotateTransform(180);
                        this.Coordinates = newCoord;
                    }
                }
                else if (Player.Coordinates.Y < this.Coordinates.Y)
                {
                    var newCoord = new Point(this.Coordinates.X, this.Coordinates.Y - 1);
                    if (!IsBulletCollisionTrajectory(newCoord))
                    {
                        this.Direction = Direction.Up;
                        UiContainer.RenderTransform = new RotateTransform(0);
                        this.Coordinates = newCoord;
                    }
                }
                if (this.Coordinates.X == Player.Coordinates.X && this.Coordinates.Y == Player.Coordinates.Y)
                {
                    GhostTimer.Stop();
                    GameTable.OnGameOver();
                }
            }
            else
            {
                TryDodgeBullet(BulletToDodge);
            }
            

            AdjustCoordinatesToFitIntoTable();
            TickCount++;
        }

        private bool TryDodgeBullet(Bullet<Ghost, Player> bulletToDodge)
        {
            bool dodged = false;
            if (bulletToDodge != null)
            {
                if(!IsBulletCollisionTrajectory(new Point(this.Coordinates.X + 1, this.Coordinates.Y)))
                {
                    dodged = true;
                    this.Coordinates = new Point(this.Coordinates.X + 1, this.Coordinates.Y);
                    UiContainer.RenderTransform = new RotateTransform(90);
                    this.Direction = Direction.Right;
                }
                else if (!IsBulletCollisionTrajectory(new Point(this.Coordinates.X - 1, this.Coordinates.Y)))
                {
                    dodged = true;
                    this.Coordinates = new Point(this.Coordinates.X - 1, this.Coordinates.Y);
                    UiContainer.RenderTransform = new RotateTransform(-90);
                    this.Direction = Direction.Left;
                }
                else if (!IsBulletCollisionTrajectory(new Point(this.Coordinates.X, this.Coordinates.Y + 1)))
                {
                    dodged = true;
                    this.Coordinates = new Point(this.Coordinates.X, this.Coordinates.Y + 1);
                    UiContainer.RenderTransform = new RotateTransform(180);
                    this.Direction = Direction.Down;
                }
                else if (!IsBulletCollisionTrajectory(new Point(this.Coordinates.X, this.Coordinates.Y - 1)))
                {
                    dodged = true;
                    this.Coordinates = new Point(this.Coordinates.X, this.Coordinates.Y - 1);
                    UiContainer.RenderTransform = new RotateTransform(0);
                    this.Direction = Direction.Up;
                }
            }

            return dodged;
        }

        private bool IsBulletCollisionTrajectory(Point newCoordinates)
        {
            Bullet<Ghost, Player> bullet;
            return IsBulletCollisionTrajectory(newCoordinates, out bullet);
        }

        private bool IsBulletCollisionTrajectory(Point newCoordinates, out Bullet<Ghost, Player> bulletToDodge)
        {
            lock (GameTable._Items)
            {
                foreach (var item in GameTable._Items)
                {
                    if (item.Value is Bullet<Ghost, Player>)
                    {
                        var bullet = (Bullet<Ghost, Player>)item.Value;

                        //if Y equals
                        if (bullet.Coordinates.Y == newCoordinates.Y)
                        {
                            if ((bullet.Coordinates.X > newCoordinates.X && bullet.Direction == Direction.Left) ||
                                (bullet.Coordinates.X < newCoordinates.X && bullet.Direction == Direction.Right)
                            )
                            {
                                bulletToDodge = bullet;
                                return true;
                            }
                        }
                        //if X equals
                        else if (bullet.Coordinates.X == newCoordinates.X)
                        {
                            if ((bullet.Coordinates.Y > newCoordinates.Y && bullet.Direction == Direction.Up) ||
                                (bullet.Coordinates.Y < newCoordinates.Y && bullet.Direction == Direction.Down)
                            )
                            {
                                bulletToDodge = bullet;
                                return true;
                            }
                        }
                    }
                }
            }
            bulletToDodge = null;
            return false;
        }

        internal override void Destroy()
        {
            GhostTimer.Stop();
            if (BulletDodgeReactionTimer != null)
            {
                BulletDodgeReactionTimer.Stop();
            }
            
            base.Destroy();
        }
    }
}
