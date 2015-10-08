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

        public void AddTile(Tile tile)
        {
            tile.BaseActor = this;
            Tiles.Add(tile);
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
                    if(tile.CurrentRectCoords.X + tile.CurrentRectCoords.Width > BaseGameBoard.BoardRectCoords.X + BaseGameBoard.BoardRectCoords.Width ||
                       tile.CurrentRectCoords.X < BaseGameBoard.BoardRectCoords.X)
                    {
                        _velocity.X = -_velocity.X;
                        break;
                    } else if (tile.CurrentRectCoords.Y + tile.CurrentRectCoords.Height > BaseGameBoard.BoardRectCoords.Y + BaseGameBoard.BoardRectCoords.Height ||
                        tile.CurrentRectCoords.Y < BaseGameBoard.BoardRectCoords.Y)
                    {
                        _velocity.Y = -_velocity.Y;
                        break;
                    }
                }
            }
            foreach (Tile tile in Tiles)
            {
                Rectangle currentRect = tile.CurrentRectCoords;
                tile.CurrentRectCoords = new Rectangle(currentRect.X + (int)Velocity.X, currentRect.Y + (int)Velocity.Y, currentRect.Width, currentRect.Height);
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
                if (tile is TileBouncer)
                {
                    MovementType = "Bouncer";
                    Random rnd1 = new Random();
                    _velocity.X = rnd1.Next(-5, 5);
                    _velocity.Y = rnd1.Next(-5, 5);
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
