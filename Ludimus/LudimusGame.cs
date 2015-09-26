﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Ludimus
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class LudimusGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Board GameBoard;
        Board UIBoard;
        Color[] UIColorList;
        Color SelectedColor;
        MouseState CurrentMouseState;
        Point CurrentMousePosition;
        GameButton PlayButton;
        GameButton StopButton;

        public LudimusGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            CurrentMouseState = Mouse.GetState();
            CurrentMousePosition = new Point(CurrentMouseState.X, CurrentMouseState.Y);

            GameBoard = new Board();
            GameBoard.Initialize(20, 12, 30, 30, new Point(100, 50), graphics);

            UIColorList = new Color[] { Color.Blue, Color.Aqua, Color.LightSkyBlue,
                                        Color.LightSeaGreen, Color.ForestGreen, Color.LightGreen,
                                       Color.Red, Color.Gold, Color.Orange, Color.HotPink,
                                       Color.Black, Color.Ivory};
            System.Console.WriteLine(UIColorList.Length);

            UIBoard = new Board();
            UIBoard.Initialize(1, UIColorList.Length, 40, 40, new Point(0, 0), graphics);
            for(int i=0; i < UIColorList.Length; i++)
            {
                UIBoard.SetColor(new Point(0, i), UIColorList[i]);
            }

            PlayButton = new GameButton();
            StopButton = new GameButton();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
            Point playButtonPosition = new Point(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - 50,
                GraphicsDevice.Viewport.TitleSafeArea.Y);
            PlayButton.Initialize(Content.Load<Texture2D>("Graphics\\PlayButton"), playButtonPosition, new Vector2(0.4f, 0.4f));

            Point stopButtonPosition = new Point(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - 10,
                GraphicsDevice.Viewport.TitleSafeArea.Y);
            StopButton.Initialize(Content.Load<Texture2D>("Graphics\\StopButton"), stopButtonPosition, new Vector2(0.4f, 0.4f));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            CurrentMouseState = Mouse.GetState();
            CurrentMousePosition = new Point(CurrentMouseState.X, CurrentMouseState.Y);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Check if play mode started
            if (PlayButton.CheckMousePosition(CurrentMousePosition) && CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                GameBoard.PlayMode = true;
                System.Console.WriteLine("Play Mode Started");
            } else if (StopButton.CheckMousePosition(CurrentMousePosition) && CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                GameBoard.PlayMode = false;
                System.Console.WriteLine("Play Mode Stopped");
            }

            Point selectedUITilePosition = UIBoard.FindSelectedTile(CurrentMousePosition);
            Point selectedBoardTilePosition = GameBoard.FindSelectedTile(CurrentMousePosition);
            if (selectedUITilePosition != default(Point) && CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                SelectedColor = UIBoard.SelectNewColor(selectedUITilePosition);
                System.Console.WriteLine("Selected new color: " + SelectedColor.ToString());
            } else if (selectedBoardTilePosition != default(Point) && CurrentMouseState.LeftButton == ButtonState.Pressed)
            {
                GameBoard.ActivateTile(selectedBoardTilePosition, SelectedColor);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            UIBoard.Draw(spriteBatch);

            PlayButton.Draw(spriteBatch);
            StopButton.Draw(spriteBatch);

            GameBoard.Draw(spriteBatch);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}