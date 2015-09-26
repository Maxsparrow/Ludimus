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

        private Tile[,] BoardTiles;
        private Point StartLocation;
        private int BoardWidth;
        private int BoardHeight;
        private int TileWidth;
        private int TileHeight;

        public void Initialize(int boardWidth, int boardHeight, int tileWidth, int tileHeight, Point startlocation, GraphicsDeviceManager graphics)
        {
            StartLocation = startlocation;
            BoardTiles = new Tile[boardWidth, boardHeight];
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Actors = new List<Actor>();

            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    BoardTiles[x, y] = new Tile();
                    BoardTiles[x, y].Initialize(new Rectangle(StartLocation.X + TileWidth * x, StartLocation.Y + TileHeight * y, 40, 40), graphics);
                }
            }
        }

        public void Update()
        {
            for (int i = 0; i < Actors.Count; i++)
            {
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    BoardTiles[x, y].Draw(spriteBatch);
                }
            }
        }

        public void SetColor(Point tilePosition, Color color)
        {
            BoardTiles[tilePosition.X, tilePosition.Y].RectColor = color;
        }
        
        public Point FindSelectedTile(Point mousePosition)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    if (BoardTiles[x, y].CheckMousePosition(mousePosition))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return default(Point);
        }

        public Color SelectNewColor(Point tilePosition)
        {
            return BoardTiles[tilePosition.X, tilePosition.Y].RectColor;
        }

        public void ActivateTile(Point tilePosition, Color newColor)
        {
            Tile selectedTile = BoardTiles[tilePosition.X, tilePosition.Y];
            if (newColor != selectedTile.DefaultColor)
            {
                selectedTile.RectColor = newColor;
                Actors.Add(new Actor(selectedTile));
            }
        }

    }
}
