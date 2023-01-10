using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerRoleplayC
{
    public class Player : Sprite
    {
        public Vector2 oldPos { get; set; }

        public GamePadState oldState { get; set; }

        public int ID;

        public Dictionary<string, int> Items { get; set; }

        public Vector2 Movement { get; set; }

        public Texture2D[] Animations { get; set; }

        public Player(Texture2D tex, Vector2 pos, SpriteBatch spriteBatch,
            Texture2D[] animations, int id) : base(tex, pos, spriteBatch)
        {
            Animations = animations;
            ID = id;
            Movement = Vector2.Zero;
            Items = new Dictionary<string, int>();Items.Add("gun", 1);Items.Add("jetpack", 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GamePadState state = GamePad.GetState(ID - 1);

            int bg = Behind();

            #region Get input
            if (state.ThumbSticks.Left.X != 0)
            {//Left and right
                if (bg == 2)
                    Movement += new Vector2(state.ThumbSticks.Left.X / 2, 0);
                else
                    Movement += new Vector2(state.ThumbSticks.Left.X, 0);

                //Right

                /*if (bg == 2)
                    Movement += new Vector2(state.ThumbSticks.Left.X / 2, 0);
                else
                    Movement += new Vector2(state.ThumbSticks.Left.X, 0);*/
            }
            if (state.Buttons.A == ButtonState.Pressed&& !CanMove(new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height + 1))
                && oldState.Buttons.A != ButtonState.Pressed)
            {//Jump
                    if (bg != 2)
                        Movement = new Vector2(Movement.X, Movement.Y - 30); //28
            }
            if (state.Triggers.Right > 0)
            {
                if (Items["jetpack"] > 0)
                {
                    Movement = new Vector2(Movement.X, Movement.Y - state.Triggers.Right * 3);
                }
            }
            if (state.ThumbSticks.Left.Y != 0)
            {//Climb up/Down
                if (bg == 2)
                {
                    Movement += new Vector2(0, -state.ThumbSticks.Left.Y);
                }
            }
            if (state.Buttons.Y == ButtonState.Pressed && oldState.Buttons.Y != ButtonState.Pressed)
            {
                if (bg == 4)
                {
                    var be = BBehind();
                    foreach (var b in Map.Current.Blocks)
                        if (b.ID == 4 && b.ExtraData != null && b.Position != be.Position)
                        {
                            if ((int)b.ExtraData[0] == (int)be.ExtraData[1])
                                Position = new Vector2(b.Position.X, b.Position.Y - b.Texture.Height);
                        }
                }
            }
            /*if (state.Buttons.RightStick == ButtonState.Pressed && oldState.Buttons.RightStick != ButtonState.Pressed)
            {
                List<int> ids = new List<int>()
                {
                    1, 2, 3, 4
                };
                foreach (var p in Map.Current.Players)
                {
                    var id = ids[RandomNumber(0, ids.Count)];
                    p.ID = id;
                    ids.Remove(id);
                }
            }*/
            if (Items["gun"] > 0)
            {
                if (state.ThumbSticks.Right.X > 0.3 && oldState.ThumbSticks.Right.X < 0.3)
                {
                    Map.Current.Sprites.Add(new Bullet(
                        Map.SpriteTextures[0],
                        new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2))
                        , SpriteBatch, "right", this));
                }
                if (state.ThumbSticks.Right.X < -0.3 && oldState.ThumbSticks.Right.X > -0.3)
                {
                    Map.Current.Sprites.Add(new Bullet(
                        Map.SpriteTextures[0],
                        new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2))
                        , SpriteBatch, "left", this));
                }
                if (state.ThumbSticks.Right.Y > 0.3 && oldState.ThumbSticks.Right.Y < 0.3)
                {
                    Map.Current.Sprites.Add(new Bullet(
                        Map.SpriteTextures[0],
                        new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2))
                        , SpriteBatch, "up", this));
                }
                if (state.ThumbSticks.Right.Y < -0.3 && oldState.ThumbSticks.Right.Y > -0.3)
                {
                    Map.Current.Sprites.Add(new Bullet(
                        Map.SpriteTextures[0],
                        new Vector2(Position.X + (Texture.Width / 2), Position.Y + (Texture.Height / 2))
                        , SpriteBatch, "down", this));
                }
            }
            #endregion

            oldPos = Position;

            if (bg == 0)
                Movement += new Vector2(0, 1.8f);

            Movement *= new Vector2(0.8f, 0.8f); //0.8f

            Vector2 newPos = Position + Movement * (float)gameTime.ElapsedGameTime.Milliseconds / 15;

            Position = Furthest(oldPos, newPos, Bounds);

            oldState = state;
        }

        public override void Draw()
        {
            if (Movement.X > 0)
                Texture = Animations[2];
            if (Movement.X < 0)
                Texture = Animations[0];

            base.Draw();
        }

        public Vector2 Furthest(Vector2 pos1, Vector2 pos2, Rectangle bounds)
        {
            Vector2 movement = pos2 - pos1;
            Vector2 furthest = pos1;
            int steps = (int)(movement.Length() * 2) + 1;
            Vector2 step = movement / steps;

            for (int i = 1; i <= steps; i++)
            {
                Vector2 posTry = pos1 + step * i;
                Rectangle rec = new Rectangle((int)posTry.X, (int)posTry.Y, bounds.Width, bounds.Height);
                if (CanMove(rec)) { furthest = posTry; }
                else
                {
                    if (movement.X != 0 && movement.Y != 0)
                    {
                        int remSteps = steps - (i - 1);
                        Vector2 remX = step.X * Vector2.UnitX * remSteps;
                        Vector2 finalx = furthest + remX;
                        furthest = Furthest(furthest, finalx, bounds);

                        Vector2 remY = step.Y * Vector2.UnitY * remSteps;
                        Vector2 finaly = furthest + remY;
                        furthest = Furthest(furthest, finaly, bounds);
                    }
                    break;
                }
            }
            return furthest;
        }

        public bool CanMove(Rectangle bounds)
        {
            foreach (var b in Map.Current.Blocks)
            {
                if (bounds.Intersects(b.Bounds) && b.Layer == 0)
                    return false;
            }
            return true;
        }

        public int Behind()
        {
            foreach (var b in Map.Current.Blocks)
                if (b.Bounds.Intersects(Bounds))
                    return b.ID;
            return 0;
        }

        public Block BBehind()
        {
            foreach (var b in Map.Current.Blocks)
                if (b.Bounds.Intersects(Bounds))
                    return b;
            return null;
        }

        public override void Interact(params object[] data)
        {
            base.Interact(data);
        }

        public bool Playing()
        {
            return GamePad.GetCapabilities(ID - 1).IsConnected;
        }

        //Function to get random number
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }
    }
}
