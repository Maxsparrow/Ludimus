using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        GameBoard GameBoardMain;
        UIBoard UIBoardColors;
        MouseState CurrentMouseState;
        Point CurrentMousePosition;
        GameButton PlayButton;
        GameButton StopButton;
        GameButton EraseButton;

        public TileType SelectedTileType = TileType.Background;

        public static readonly Dictionary<Enum, Color> TileTypeLookup =
            new Dictionary<Enum, Color>()
            {
                { TileType.Background, Color.Ivory },
                { TileType.Basic, Color.Gray },
                { TileType.Bouncer, Color.LightGreen }
            };

        public static readonly Dictionary<Tuple<ActorMovementType, ActorMovementType>, Enum> ActorCollisionLookup =
            new Dictionary<Tuple<ActorMovementType, ActorMovementType>, Enum>()
            {
                    { new Tuple<ActorMovementType, ActorMovementType>( ActorMovementType.Bouncer, ActorMovementType.Basic ), ActorCollisionType.Bounce },
                     { new Tuple<ActorMovementType, ActorMovementType>( ActorMovementType.Bouncer, ActorMovementType.Bouncer ), ActorCollisionType.BounceBoth }
            };

        public LudimusGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
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

            GameBoardMain = new GameBoard(this);
            GameBoardMain.Initialize(20, 16, 30, 30, new Point(100, 50), graphics);
            
            TileType[] tileTypes = (TileType[])Enum.GetValues(typeof(TileType));

            UIBoardColors = new UIBoard(this);
            UIBoardColors.Initialize(1, tileTypes.Length, 40, 40, new Point(0, 0), graphics);
            for(int i=0; i < tileTypes.Length; i++)
            {
                UIBoardColors.SetType(i, tileTypes[i]);
            }

            PlayButton = new GameButton();
            StopButton = new GameButton();
            EraseButton = new GameButton();

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
            
            Point eraseButtonPosition = new Point(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 8,
                GraphicsDevice.Viewport.TitleSafeArea.Y);
            EraseButton.Initialize(Content.Load<Texture2D>("Graphics\\EraseButton"), eraseButtonPosition, new Vector2(0.4f, 0.4f));
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
            if (PlayButton.CheckMousePosition(CurrentMousePosition) && CurrentMouseState.LeftButton == ButtonState.Pressed && !GameBoardMain.PlayMode)
            {
                GameBoardMain.EnablePlayMode();
                System.Console.WriteLine("Play Mode Started");
            } else if (StopButton.CheckMousePosition(CurrentMousePosition) && CurrentMouseState.LeftButton == ButtonState.Pressed && GameBoardMain.PlayMode)
            {
                GameBoardMain.DisablePlayMode();
                System.Console.WriteLine("Play Mode Stopped");
            }

            // Check if Erase Button pushed
            if (EraseButton.CheckMousePosition(CurrentMousePosition) && CurrentMouseState.LeftButton == ButtonState.Pressed && !GameBoardMain.PlayMode)
            {
                GameBoardMain.EraseAll();
                System.Console.WriteLine("Board Erased");
            }

            UIBoardColors.Update();
            GameBoardMain.Update();

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

            UIBoardColors.Draw(spriteBatch);

            PlayButton.Draw(spriteBatch);
            StopButton.Draw(spriteBatch);
            EraseButton.Draw(spriteBatch);

            GameBoardMain.Draw(spriteBatch);

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
