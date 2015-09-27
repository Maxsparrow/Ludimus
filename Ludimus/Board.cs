using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class Board
    {
        public List<Actor> Actors;
        public bool PlayMode { get; set; }
        private List<Tile> BoardTiles;
        private Point StartLocation;
        private int BoardWidth;
        private int BoardHeight;
        private int TileWidth;
        private int TileHeight;
        private GraphicsDeviceManager Graphics;

        public void Initialize(int boardWidth, int boardHeight, int tileWidth, int tileHeight, Point startlocation, GraphicsDeviceManager graphics)
        {
            StartLocation = startlocation;
            BoardTiles = new List<Tile>();
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Actors = new List<Actor>();
            Graphics = graphics;

            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    Tile tileToAdd = new Tile();
                    tileToAdd.Initialize(new Rectangle(StartLocation.X + TileWidth * x, StartLocation.Y + TileHeight * y, TileWidth, TileHeight), graphics);
                    BoardTiles.Add(tileToAdd);
                }
            }
        }

        public void EnablePlayMode()
        {
            PlayMode = true;
            foreach(Tile tile in BoardTiles)
            {
                tile.DisableBorders();
            }
        }
        public void DisablePlayMode()
        {
            PlayMode = false;
            foreach (Tile tile in BoardTiles)
            {
                tile.EnableBorders();
            }
        }

        public void Update()
        {
            foreach(Actor actor in Actors)
            {
                actor.Move(1);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in BoardTiles)
            {
                tile.Draw(spriteBatch);
            }
            foreach (Actor actor in Actors)
            {
                actor.Draw(spriteBatch);
            }

        }

        public void SetColor(int tileNumber, Color color)
        {
            BoardTiles[tileNumber].RectColor = color;
        }
        
        public Tile FindSelectedTile(Point mousePosition)
        {
            foreach (Tile tile in BoardTiles)
            {
                if (tile.CheckMousePosition(mousePosition))
                {
                    return tile;
                }
            }
            return null;
        }

        public void ActivateTile(Rectangle rectCoords, Color newColor)
        {
            if (newColor != Tile.DefaultColor)
            {
                Actors.Add(new Actor(rectCoords, Graphics, newColor));
            }
            else if(newColor == Tile.DefaultColor)
            {
                for (int i = 0; i < Actors.Count; i++)
                {
                    if (Actors[i].CheckMousePosition(new Point(rectCoords.X, rectCoords.Y)))
                    {
                        Actors.Remove(Actors[i]);
                    }
                }
            }
        }

    }
}
