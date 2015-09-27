using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class Actor
    {
        public List<Tile> Tiles;

        public Actor(Rectangle rectCoords, GraphicsDeviceManager graphics, Color color)
        {
            Tiles = new List<Tile>();
            Tile tileToAdd = new Tile();
            tileToAdd.Initialize(rectCoords, graphics, color);
            Tiles.Add(tileToAdd);
        }
        
        public void Initialize()
        {
        }

        public void Move(float speed)
        {
            foreach(Tile tile in Tiles)
            {
                Rectangle currentRect = tile.RectCoords;
                tile.RectCoords = new Rectangle(currentRect.X + (int)speed, currentRect.Y, currentRect.Width, currentRect.Height);
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

    }
}
