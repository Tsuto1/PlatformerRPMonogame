using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerRoleplayC
{
    public class Bullet : Sprite
    {
        public string Direction { get; set; }

        public Player Shooter { get; set; }

        public Bullet(Texture2D tex, Vector2 pos, SpriteBatch spriteBatch, string direction, Player shooter, float rotation = 0) : base(tex, pos, spriteBatch)
        {
            Direction = direction;
            Shooter = shooter;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 oldPos = Position;

            switch (Direction)
            {
                case "up":
                    Position += (-Vector2.UnitY) * 5;
                    break;
                case "left":
                    Position += (-Vector2.UnitX)* 5;
                    break;
                case "down":
                    Position += (Vector2.UnitY)*5;
                    break;
                case "right":
                    Position += (Vector2.UnitX)*5;
                    break;
            }

            Rectangle newBounds = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            if (!CanMove(newBounds))
            {
                /*
                 * foreach (var s in Map.Current.Sprites)
                 *     if (s.Texture == Map.Current.SpriteTextures[*insert shooting board id here*])
                 *         s.Send("shot");
                 * Map.Current.Sprites.Remove(this);
                 */
                foreach (var b in Map.Current.Blocks)
                    if (b.Bounds.Intersects(newBounds) && b.ID == 5)
                        b.Interact("shot", Shooter);
                Remove = true;// Map.Current.Sprites.Remove(this);
                
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {
            SpriteBatch.Draw(Texture, Position, Color.White);

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
    }
}
