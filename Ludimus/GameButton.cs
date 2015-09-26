using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ludimus
{
    class GameButton
    {
        private Texture2D ButtonTexture;
        private Point ButtonPosition;
        private Vector2 ButtonScale;
        private Rectangle RectCoords;

        public void Initialize(Texture2D buttonTexture, Point buttonPosition, Vector2 buttonScale)
        {
            ButtonTexture = buttonTexture;
            ButtonPosition = buttonPosition;
            ButtonScale = buttonScale;

            RectCoords = new Rectangle(buttonPosition.X, buttonPosition.Y, (int)(ButtonTexture.Width* ButtonScale.X), (int)(ButtonTexture.Height*buttonScale.Y));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ButtonTexture, RectCoords, color:Color.White);
        }


        public bool CheckMousePosition(Point mousePosition)
        {
            return RectCoords.Contains(mousePosition) ? true : false;
        }
    }
}
