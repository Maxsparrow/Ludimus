using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ludimus
{
    class UIBoard
    {
        private LudimusGame BaseGame;

        private List<Tile> BoardTiles;
        private Point StartLocation;
        private int BoardWidth;
        private int BoardHeight;
        private int TileWidth;
        private int TileHeight;
        private GraphicsDeviceManager Graphics;

        public UIBoard(LudimusGame baseGame)
        {
            BaseGame = baseGame;
        }

        public void Initialize(int boardWidth, int boardHeight, int tileWidth, int tileHeight, Point startlocation, GraphicsDeviceManager graphics)
        {
            StartLocation = startlocation;
            BoardTiles = new List<Tile>();
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Graphics = graphics;

            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    Tile tileToAdd = new Tile();
                    tileToAdd.Initialize(new Point(StartLocation.X + TileWidth * x, StartLocation.Y + TileHeight * y), new Point(TileWidth, TileHeight), graphics);
                    BoardTiles.Add(tileToAdd);
                }
            }
        }
        
        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();
            Point currentMousePosition = new Point(currentMouseState.X, currentMouseState.Y);

            Tile selectedUITile = FindSelectedTile(currentMousePosition);
            if (selectedUITile != null && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                BaseGame.SelectedTileType = selectedUITile.Type;
                System.Console.WriteLine("Selected new color: " + LudimusGame.TileTypeLookup[BaseGame.SelectedTileType].ToString());
                System.Console.WriteLine("Selected tile type: " + BaseGame.SelectedTileType.ToString());
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in BoardTiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        public void SetType(int tileNumber, TileType tileType)
        {
            BoardTiles[tileNumber].Type = tileType;
            BoardTiles[tileNumber].RectColor = LudimusGame.TileTypeLookup[tileType];
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

    }
}
