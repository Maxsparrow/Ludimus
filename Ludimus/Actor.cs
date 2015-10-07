using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class Actor
    {
        public List<Tile> Tiles;
        public float Velocity { get; set; }

        public void AddTile(Rectangle rectCoords, GraphicsDeviceManager graphics, Color color)
        {
            Tile tileToAdd = new Tile();
            tileToAdd.Initialize(rectCoords, graphics, color);
            Tiles.Add(tileToAdd);
        }
        
        public Actor()
        {
            Tiles = new List<Tile>();
            Velocity = 1;
        }

        public void Move(Rectangle boardRectCoords)
        {
            foreach (Tile tile in Tiles)
            {
                if (!boardRectCoords.Contains(tile.RectCoords))
                {
                    Velocity = -Velocity;
                }
            }
            foreach (Tile tile in Tiles)
            {
                Rectangle currentRect = tile.RectCoords;
                tile.RectCoords = new Rectangle(currentRect.X + (int)Velocity, currentRect.Y, currentRect.Width, currentRect.Height);
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
