using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class Actor
    {
        public List<Tile> Tiles;

        public Actor(Tile boardTile)
        {
            Tiles = new List<Tile>();
            Tiles.Add(boardTile);
        }


        public void Initialize()
        {
        }

        public void Move()
        {
            foreach(Tile tile in Tiles )
            {
            }
        }

    }
}
