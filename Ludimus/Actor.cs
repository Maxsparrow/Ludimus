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
        private Vector2 _velocity;
        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }
        public string MovementType;

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
                Rectangle currentRect = tile.CurrentRectCoords;
                Point currentGlobalPosition = tile.CurrentGlobalPosition;
                tile.CurrentGlobalPosition = new Point(currentGlobalPosition.X + (int)Velocity.X, currentGlobalPosition.Y + (int)Velocity.Y);
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
                    MovementType = "Bouncer";
                    _velocity.X = GameBoard.RandomGenerator.Next(-5, 5);
                    _velocity.Y = GameBoard.RandomGenerator.Next(-5, 5);
                    break;
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
