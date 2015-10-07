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

        private Rectangle _boardRectCoords;
        public Rectangle BoardRectCoords { get { return _boardRectCoords; } }

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
            FindBoardRectCoords();
            foreach(Actor actor in Actors)
            {
                actor.Move(BoardRectCoords);
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

        public bool ActivateTile(Rectangle rectCoords, Color newColor)
        {
            if (newColor != Tile.DefaultColor)
            {
                // Remove any existing Actors, before adding new one
                /*

                for (int i = 0; i < Actors.Count; i++)
                {
                    if (Actors[i].CheckMousePosition(new Point(rectCoords.X, rectCoords.Y)))
                    {
                        Actors.Remove(Actors[i]);
                    }
                }*/

                // Check if a neighboring tile already has an Actor
                for (int i = 0; i < Actors.Count; i++)
                {
                    if (Actors[i].CheckIfNeighboring(rectCoords))
                    {
                        Actors[i].AddTile(rectCoords, Graphics, newColor);
                        return true;
                    }
                }
                Actor actorToAdd = new Actor();
                actorToAdd.AddTile(rectCoords, Graphics, newColor);
                Actors.Add(actorToAdd);
                return true;
            }
            else if(newColor == Tile.DefaultColor)
            {
                //If default base color, remove existing Actors
                for (int i = 0; i < Actors.Count; i++)
                {
                    if (Actors[i].CheckMousePosition(new Point(rectCoords.X, rectCoords.Y)))
                    {
                        Actors.Remove(Actors[i]);
                    }
                }
                return false;
            }
            return false;
        }

        private void FindBoardRectCoords()
        {
            int maxX = 0;
            int maxY = 0;
            int minX = 99999;
            int minY = 99999;
            int tileWidth = 0;
            int tileHeight = 0;
            foreach (Tile tile in BoardTiles)
            {
                if (tile.RectCoords.X > maxX)
                    maxX = tile.RectCoords.X;
                    tileWidth = tile.RectCoords.Width;
                    tileHeight = tile.RectCoords.Height;
                if (tile.RectCoords.X < minX)
                    minX = tile.RectCoords.X;
                if (tile.RectCoords.Y > maxY)
                    maxY = tile.RectCoords.Y;
                if (tile.RectCoords.Y < minY)
                    minY = tile.RectCoords.Y;
            }

            _boardRectCoords = new Rectangle(minX, minY, maxX - minX + tileWidth, maxY - minY + tileHeight);
        }

    }
}
