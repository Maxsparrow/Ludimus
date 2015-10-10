using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class Tile
    {
        private Color _rectColor;
        public Color RectColor
        {   get
            {
                return _rectColor;
            }
            set
            {
                _rectColor = value;
                // If RectTexture has been set, make sure to change it's color
                if(RectTexture != default(Texture2D))
                    RectTexture.SetData(new[] { value });
            }
        }

        private static Color _defaultColor = Color.Ivory;
        public static Color DefaultColor { get { return _defaultColor; } }

        private static int _defaultBorderWidth = 1;
        public static int DefaultBorderWidth { get { return _defaultBorderWidth; } }

        public Color BorderColor { get; set; }
        public int BorderWidth { get; set; }

        public Point BoardPosition { get; set; }
        public Rectangle CurrentRectCoords { get; set; }
        public Rectangle OriginalRectCoords { get; set; }
        private Texture2D BorderTexture;
        private Texture2D RectTexture;

        private GraphicsDeviceManager graphics;

        public Actor BaseActor { get; set; }
        public TileType Type { get; set; }

        public void Initialize(Rectangle coords, GraphicsDeviceManager graphicsManager, Color color = default(Color))
        {
            // Set default Border settings
            BorderColor = Color.Black;
            BorderWidth = 1;

            // Set default Color settings
            if (color != default(Color))
                RectColor = color;
            else
                RectColor = DefaultColor;

            graphics = graphicsManager;

            // Create internal rectangle texture
            RectTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            RectTexture.SetData(new[] { RectColor });

            // Create border rectangle texture
            BorderTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            BorderTexture.SetData(new[] { BorderColor });
            
            CurrentRectCoords = coords;
            OriginalRectCoords = CurrentRectCoords;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BorderTexture, CurrentRectCoords, Color.White);
            Rectangle innerRectangle = new Rectangle(CurrentRectCoords.X + BorderWidth, CurrentRectCoords.Y + BorderWidth, CurrentRectCoords.Width - BorderWidth * 2, CurrentRectCoords.Height - BorderWidth * 2);
            spriteBatch.Draw(RectTexture, innerRectangle, Color.White);
        }

        public void DisableBorders()
        {
            BorderWidth = 0;
        }
        public void EnableBorders()
        {
            BorderWidth = DefaultBorderWidth;
        }

        public bool CheckMousePosition(Point mousePosition)
        {
            return CurrentRectCoords.Contains(mousePosition) ? true : false;
        }

        public bool CheckIfNeighboring(Rectangle rectCoords)
        {
            if (CheckMousePosition(new Point(rectCoords.X - rectCoords.Width, rectCoords.Y)) ||
                CheckMousePosition(new Point(rectCoords.X, rectCoords.Y - rectCoords.Height)) ||
                CheckMousePosition(new Point(rectCoords.X + rectCoords.Width, rectCoords.Y)) ||
                CheckMousePosition(new Point(rectCoords.X, rectCoords.Y + rectCoords.Height)))
            {
                return true;
            }
            return false;
        }
    }
}
