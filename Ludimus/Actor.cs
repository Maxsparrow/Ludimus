﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class Actor
    {
        public List<Tile> Tiles;
        private Vector2 _velocity;
        public Vector2 Velocity {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public void AddTile(Tile tile)
        {
            tile.BaseActor = this;
            Tiles.Add(tile);
        }
        
        public Actor()
        {
            Tiles = new List<Tile>();
        }

        public void BounceMove(Rectangle boardRectCoords)
        {
            foreach (Tile tile in Tiles)
            {
                if (!boardRectCoords.Contains(tile.CurrentRectCoords))
                {
                    if(tile.CurrentRectCoords.X + tile.CurrentRectCoords.Width > boardRectCoords.X + boardRectCoords.Width ||
                       tile.CurrentRectCoords.X < boardRectCoords.X)
                    {
                        _velocity.X = -_velocity.X;
                        break;
                    } else if (tile.CurrentRectCoords.Y + tile.CurrentRectCoords.Height > boardRectCoords.Y + boardRectCoords.Height ||
                        tile.CurrentRectCoords.Y < boardRectCoords.Y)
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
