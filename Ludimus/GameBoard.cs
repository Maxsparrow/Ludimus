﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ludimus
{
    class GameBoard
    {
        private LudimusGame BaseGame;

        public List<Actor> Actors;
        public bool PlayMode { get; set; }
        private List<Tile> BoardTiles;
        private List<Tile> ActiveTiles;
        private Point StartLocation;
        private int BoardWidth;
        private int BoardHeight;
        private int TileWidth;
        private int TileHeight;
        private GraphicsDeviceManager Graphics;

        public Rectangle BoardRectCoords { get { return FindBoardRectCoords(); } }
        public static Random RandomGenerator = new Random();


        public GameBoard(LudimusGame baseGame)
        {
            BaseGame = baseGame;
        }

        public void Initialize(int boardWidth, int boardHeight, int tileWidth, int tileHeight, Point startlocation, GraphicsDeviceManager graphics)
        {
            StartLocation = startlocation;
            BoardTiles = new List<Tile>();
            ActiveTiles = new List<Tile>();
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
                    tileToAdd.Initialize(new Point(StartLocation.X + TileWidth * x, StartLocation.Y + TileHeight * y), new Point(TileWidth, TileHeight), graphics);
                    tileToAdd.BoardPosition = new Point(x, y);
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
            SetActors();
        }

        public void DisablePlayMode()
        {
            PlayMode = false;
            foreach (Tile tile in BoardTiles)
            {
                tile.EnableBorders();
            }

            // Move all ActiveTiles back to the original position
            foreach (Tile activeTile in ActiveTiles)
            {
                activeTile.CurrentGlobalPosition = activeTile.OriginalGlobalPosition;
            }

            // Destroy actors, will reset them at EnablePlayMode
            ResetActors();
        }

        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();
            Point currentMousePosition = new Point(currentMouseState.X, currentMouseState.Y);

            if (PlayMode)
            {
                FindBoardRectCoords();
                foreach (Actor actor in Actors)
                {
                    actor.Update();
                }
                HandleCollisions();
            }
            else if (!PlayMode)
            {
                // Select new colors
                Tile selectedBoardTile = FindSelectedTile(currentMousePosition);
                if (selectedBoardTile != null && currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    ActivateTile(selectedBoardTile, BaseGame.SelectedTileType);
                }
            }
        }

        public void HandleCollisions()
        {
            foreach (Actor actor1 in Actors)
            {
                foreach (Actor actor2 in Actors)
                {
                    if (actor1 != actor2)
                    {
                        CheckForCollision(actor1, actor2);
                    }
                }
            }
        }

        public void CheckForCollision(Actor actor1, Actor actor2)
        {
            bool collisionHandled = false;
            foreach(Tile tile1 in actor1.Tiles)
            {
                foreach(Tile tile2 in actor2.Tiles)
                {
                    if (tile1.CurrentRectCoords.Intersects(tile2.CurrentRectCoords))
                    {
                        Tuple<ActorMovementType, ActorMovementType> actorMovementTuple = new Tuple<ActorMovementType, ActorMovementType>(tile1.BaseActor.MovementType, tile2.BaseActor.MovementType);

                        if (LudimusGame.ActorCollisionLookup.ContainsKey(actorMovementTuple))
                        {
                            ActorCollisionType actorCollisionType = (ActorCollisionType)LudimusGame.ActorCollisionLookup[actorMovementTuple];
                            System.Console.WriteLine("Found collision Type: " + actorCollisionType.ToString());
                            if (actorCollisionType == ActorCollisionType.Bounce)
                            {
                                tile1.BaseActor.Bounce(tile1, tile2);
                                collisionHandled = true;
                                break;
                            }
                            if (actorCollisionType == ActorCollisionType.BounceBoth)
                            {
                                tile1.BaseActor.Bounce(tile1, tile2);
                                tile2.BaseActor.Bounce(tile2, tile1);
                                collisionHandled = true;
                                break;
                            }
                        }

                        System.Console.WriteLine("Colliding! " + tile1.Type + " hit " + tile2.Type);
                        System.Console.WriteLine("Colliding Actors! " + actor1.MovementType + " hit " + actor2.MovementType);
                    }
                }
                if (collisionHandled)
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in BoardTiles)
            {
                tile.Draw(spriteBatch);
            }

            if (PlayMode)
            {
                foreach (Actor actor in Actors)
                {
                    actor.Draw(spriteBatch);
                }
            }
            else if (!PlayMode)
            {
                foreach (Tile tile in ActiveTiles)
                {
                    tile.Draw(spriteBatch);
                }
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

        public void ActivateTile(Tile selectedBoardTile, TileType newTileType)
        {
            if (newTileType != TileType.Background)
            {
                //Remove any existing tiles
                Tile tileToRemove = null;
                foreach (Tile tile in ActiveTiles)
                {
                    if (tile.BoardPosition == selectedBoardTile.BoardPosition)
                    {
                        tileToRemove = tile;
                    }
                }
                if (tileToRemove != null)
                {
                    ActiveTiles.Remove(tileToRemove);
                }

                //Add a new active tile
                Tile tileToAdd = new Tile();
                if (LudimusGame.TileTypeLookup.ContainsKey(newTileType))
                {
                    tileToAdd.Type = newTileType;
                } 
                tileToAdd.Initialize(selectedBoardTile.CurrentGlobalPosition, selectedBoardTile.Size, Graphics, LudimusGame.TileTypeLookup[newTileType]);
                tileToAdd.BoardPosition = selectedBoardTile.BoardPosition;
                ActiveTiles.Add(tileToAdd);
            }
            else if(newTileType == TileType.Background)
            {
                //Remove any current active tile at this position
                Tile tileToRemove = null;
                foreach(Tile tile in ActiveTiles)
                {
                    if (tile.BoardPosition == selectedBoardTile.BoardPosition)
                    {
                        tileToRemove = tile;
                    }
                }
                if (tileToRemove != null)
                    ActiveTiles.Remove(tileToRemove);
            }
        }

        public void EraseAll()
        {
            ActiveTiles = new List<Tile>();
        }

        public void SetActors()
        {
            //Find tiles neighboring each tile, and add them to Actors
            foreach(Tile tileToCheck in ActiveTiles)
            { 
                if (tileToCheck.BaseActor == null)
                {
                    Actor actorToAdd = new Actor();
                    actorToAdd.AddTile(tileToCheck);

                    bool noTilesToAdd = false;

                    while (noTilesToAdd == false)
                    {
                        List<Tile> neighboringTiles = new List<Tile>();
                        foreach (Tile tile in actorToAdd.Tiles)
                        {
                            neighboringTiles.AddRange(FindNeighboringTiles(tile));
                        }

                        if (neighboringTiles.Count == 0)
                        {
                            noTilesToAdd = true;
                        }
                        else
                        {
                            bool addedAnyTile = false;
                            foreach (Tile tile in neighboringTiles)
                            {
                                if (actorToAdd.AddTile(tile))
                                    addedAnyTile = true;
                            }
                            if (!addedAnyTile)
                                noTilesToAdd = true;
                        }
                    }
                    actorToAdd.SetMovementType();
                    actorToAdd.BaseGameBoard = this;

                    Actors.Add(actorToAdd);
                }
            }
        }

        public void ResetActors()
        {
            foreach (Tile tile in ActiveTiles)
            {
                tile.BaseActor = null;
            }
            Actors = new List<Actor>();
        }

        public List<Tile> FindNeighboringTiles(Tile tileToCheck)
        {
            List<Tile> neighboringTiles = new List<Tile>();
            foreach(Tile tile in ActiveTiles)
            {
                if (tileToCheck.BoardPosition == new Point(tile.BoardPosition.X, tile.BoardPosition.Y - 1) ||
                    tileToCheck.BoardPosition == new Point(tile.BoardPosition.X, tile.BoardPosition.Y + 1) ||
                    tileToCheck.BoardPosition == new Point(tile.BoardPosition.X - 1, tile.BoardPosition.Y) ||
                    tileToCheck.BoardPosition == new Point(tile.BoardPosition.X + 1, tile.BoardPosition.Y))
                {
                    neighboringTiles.Add(tile);
                }
            }
            return neighboringTiles;
        }

        private Rectangle FindBoardRectCoords()
        {
            int maxX = 0;
            int maxY = 0;
            int minX = 99999;
            int minY = 99999;
            int tileWidth = 0;
            int tileHeight = 0;
            foreach (Tile tile in BoardTiles)
            {
                if (tile.CurrentRectCoords.X > maxX)
                    maxX = tile.CurrentRectCoords.X;
                    tileWidth = tile.CurrentRectCoords.Width;
                    tileHeight = tile.CurrentRectCoords.Height;
                if (tile.CurrentRectCoords.X < minX)
                    minX = tile.CurrentRectCoords.X;
                if (tile.CurrentRectCoords.Y > maxY)
                    maxY = tile.CurrentRectCoords.Y;
                if (tile.CurrentRectCoords.Y < minY)
                    minY = tile.CurrentRectCoords.Y;
            }

            return new Rectangle(minX, minY, maxX - minX + tileWidth, maxY - minY + tileHeight);
        }

    }
}
