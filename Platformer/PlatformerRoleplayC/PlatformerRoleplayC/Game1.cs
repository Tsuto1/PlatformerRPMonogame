using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace PlatformerRoleplayC
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D[] blockTextures;
        Dictionary<int, Texture2D[]> playerAnimations;
        Map Map;
        public static SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1320;//88
            graphics.PreferredBackBufferHeight = 690;//46
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("SFont");
            #region Player textures
            playerAnimations = new Dictionary<int, Texture2D[]>();
            playerAnimations.Add(1, new Texture2D[]
                { Content.Load<Texture2D>("Players/player1_left_fall"),
                    Content.Load<Texture2D>("Players/player1_left_rise"),
                    Content.Load<Texture2D>("Players/player1_right_fall"),
                    Content.Load<Texture2D>("Players/player1_right_rise") });
            playerAnimations.Add(2, new Texture2D[]
                { Content.Load<Texture2D>("Players/player2_left_fall"),
                    Content.Load<Texture2D>("Players/player2_left_rise"),
                    Content.Load<Texture2D>("Players/player2_right_fall"),
                    Content.Load<Texture2D>("Players/player2_right_rise") });
            playerAnimations.Add(3, new Texture2D[]
                { Content.Load<Texture2D>("Players/player3_left_fall"),
                    Content.Load<Texture2D>("Players/player3_left_rise"),
                    Content.Load<Texture2D>("Players/player3_right_fall"),
                    Content.Load<Texture2D>("Players/player3_right_rise") });
            playerAnimations.Add(4, new Texture2D[]
                { Content.Load<Texture2D>("Players/player4_left_fall"),
                    Content.Load<Texture2D>("Players/player4_left_rise"),
                    Content.Load<Texture2D>("Players/player4_right_fall"),
                    Content.Load<Texture2D>("Players/player4_right_rise") });
            #endregion
            #region Block textures
            blockTextures = new Texture2D[] {
                Content.Load<Texture2D>("Blocks/0_empty"),
                Content.Load<Texture2D>("Blocks/1_grass"),
                Content.Load<Texture2D>("Blocks/2_ladder"),
                Content.Load<Texture2D>("Blocks/3_gray_bricks"),
                Content.Load<Texture2D>("Blocks/4_teleporter"),
                Content.Load<Texture2D>("Blocks/5_shooting_target")
            };
            #endregion
            #region Initialize map
            Map = new Map(
                88, 46, blockTextures, spriteBatch);
            Map.Load(@"C:\D\Files\Programming\Self contained\Platformer\map.txt", @"C:\D\Files\Programming\Self contained\Platformer\mapextradata.txt");
            #endregion
            #region Initialize players
            Map.Players = new Player[] {
                new Player(playerAnimations[1][0],
                new Vector2(60, 60),
                spriteBatch,playerAnimations[1], 1),

                new Player(playerAnimations[2][0],
                new Vector2(90, 60),
                spriteBatch,playerAnimations[2], 2),

                new Player(playerAnimations[3][0],
                new Vector2(120, 60),
                spriteBatch,playerAnimations[3], 3),

                new Player(playerAnimations[4][0],
                new Vector2(150, 60),
                spriteBatch,playerAnimations[4], 4)
            };
            #endregion
            #region Initialize sprites
            Map.SpriteTextures = new Texture2D[] {
                Content.Load<Texture2D>("Sprites/0_bullet")
            };
            //Map.Sprites.Add();y
            #endregion
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/ Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach (var p in Map.Players)
                p.Update(gameTime);

            Map.Update(gameTime);

            int count = 1;
            Vector2 cam = Map.Players[0].Position;
            if (Map.Players[1].Playing()) { cam += Map.Players[1].Position; count++; }
            if (Map.Players[2].Playing()) { cam += Map.Players[2].Position; count++; }
            if (Map.Players[3].Playing()) { cam += Map.Players[3].Position; count++; }
            cam /= new Vector2(count);

            cam -= new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Map.Camera = cam;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            Map.Draw();
            foreach (var p in Map.Players)
                p.Draw();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
