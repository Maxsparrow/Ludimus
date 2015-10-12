using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class Actor
    {
        public GameBoard BaseGameBoard { get; set; }

        public List<Tile> Tiles;
        private Point _velocity;
        public Point Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public ActorMovementType MovementType;

        public bool AddTile(Tile tile)
        {
            if (!Tiles.Contains(tile))
            {
                tile.BaseActor = this;
                Tiles.Add(tile);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public Actor()
        {
            Tiles = new List<Tile>();
        }

        public void Move()
        {
            foreach (Tile tile in Tiles)
            {
                if (!BaseGameBoard.BoardRectCoords.Contains(tile.CurrentRectCoords))
                {
                    if(tile.CurrentGlobalPosition.X + tile.Size.X > BaseGameBoard.BoardRectCoords.X + BaseGameBoard.BoardRectCoords.Width ||
                       tile.CurrentGlobalPosition.X < BaseGameBoard.BoardRectCoords.X)
                    {
                        _velocity.X = -_velocity.X;
                        break;
                    } else if (tile.CurrentGlobalPosition.Y + tile.Size.Y > BaseGameBoard.BoardRectCoords.Y + BaseGameBoard.BoardRectCoords.Height ||
                        tile.CurrentGlobalPosition.Y < BaseGameBoard.BoardRectCoords.Y)
                    {
                        _velocity.Y = -_velocity.Y;
                        break;
                    }
                }
            }

            foreach (Tile tile in Tiles)
            {
                tile.CurrentGlobalPosition = new Point(tile.CurrentGlobalPosition.X + (int)Velocity.X, tile.CurrentGlobalPosition.Y + (int)Velocity.Y);
            }
        }

        // When colliding with another tile, reverse movement by one step
        public void Backup()
        {
            foreach (Tile tile in Tiles)
            {
                tile.CurrentGlobalPosition = new Point(tile.CurrentGlobalPosition.X - (int)Velocity.X, tile.CurrentGlobalPosition.Y - (int)Velocity.Y);
            }
        }

        public void Bounce(Tile selfTile, Tile otherTile)
        {
            Backup();

            // Logic to decide when to switch directions
            if ((otherTile.CurrentRectCoords.X >= selfTile.CurrentRectCoords.X + selfTile.CurrentRectCoords.Width ||
                otherTile.CurrentRectCoords.X + otherTile.CurrentRectCoords.Width <= selfTile.CurrentRectCoords.X) && !(otherTile.CurrentRectCoords.Y >= selfTile.CurrentRectCoords.Y + selfTile.CurrentRectCoords.Height ||
                otherTile.CurrentRectCoords.Y + otherTile.CurrentRectCoords.Height <= selfTile.CurrentRectCoords.Y))
                _velocity.X *= -1;
            else if ((otherTile.CurrentRectCoords.Y >= selfTile.CurrentRectCoords.Y + selfTile.CurrentRectCoords.Height ||
                otherTile.CurrentRectCoords.Y + otherTile.CurrentRectCoords.Height <= selfTile.CurrentRectCoords.Y) && !(otherTile.CurrentRectCoords.X >= selfTile.CurrentRectCoords.X + selfTile.CurrentRectCoords.Width ||
                otherTile.CurrentRectCoords.X + otherTile.CurrentRectCoords.Width <= selfTile.CurrentRectCoords.X))
                _velocity.Y *= -1;
            else
            {
                // Multiplying by different numbers here to prevent infinite bounce loops
                // TODO: Try to improve this logic as it can lead to unexpected bounce results
                _velocity.X = (int)(_velocity.X * -0.5f);
                _velocity.Y = (int)(_velocity.Y * -1.5f);
            }

            // Prevent stopping or stuck tiles by randomly adding a positive or negative small velocity 
            if (_velocity.X == 0)
            {
                int rand = GameBoard.RandomGenerator.Next(0, 1);
                System.Console.WriteLine(rand);
                if (rand == 0)
                    _velocity.X += 1;
                else
                    _velocity.X -= 1;
            }
            if (_velocity.Y == 0)
            {
                int rand = GameBoard.RandomGenerator.Next(0, 1);
                if (rand == 0)
                    _velocity.Y += 1;
                else
                    _velocity.Y -= 1;
            }
        }

        public void SetMovementType()
        {
            // Defines movement for the whole actor
            foreach (Tile tile in Tiles)
            {
                // if tile is TileBouncer, set it to bouncer, etc.
                // Check for conflicts, or rather have override hierarchy

                //Give random starting velocity for all actors
                if (tile.Type == TileType.Bouncer)
                {
                    MovementType = ActorMovementType.Bouncer;
                    _velocity.X = GameBoard.RandomGenerator.Next(-5, 5);
                    _velocity.Y = GameBoard.RandomGenerator.Next(-5, 5);
                    break;
                } else
                {
                    MovementType = ActorMovementType.Basic;
                }
            }
        }

        public void Update()
        {
            // Move based on it's MovementType, use separate move function?
            Move();
            // Check all tiles for collisions?? May do that in a separate function
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in Tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        public bool CheckMousePosition(Point mousePosition)
        {
            foreach(Tile tile in Tiles)
            {
                if (tile.CheckMousePosition(mousePosition))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckIfNeighboring(Rectangle rectCoords)
        {
            foreach (Tile tile in Tiles)
            {
                if (tile.CheckIfNeighboring(rectCoords))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
