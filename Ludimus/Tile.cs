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

        private Color _defaultColor = Color.Ivory;
        public Color DefaultColor { get { return _defaultColor; } }

        public Color BorderColor { get; set; }
        public int BorderWidth { get; set; }

        private Rectangle RectCoords;
        private Texture2D BorderTexture;
        private Texture2D RectTexture;

        private GraphicsDeviceManager graphics;

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
            
            RectCoords = coords;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BorderTexture, RectCoords, Color.White);
            Rectangle innerRectangle = new Rectangle(RectCoords.X + BorderWidth, RectCoords.Y + BorderWidth, RectCoords.Width - BorderWidth * 2, RectCoords.Height - BorderWidth * 2);
            spriteBatch.Draw(RectTexture, innerRectangle, Color.White);
        }

        public bool CheckMousePosition(Point mousePosition)
        {
            return RectCoords.Contains(mousePosition) ? true : false;
        }
    }
}
