using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerRoleplayC
{
    public class Sprite
    {
        public Rectangle Bounds { get; set; }

        public bool Remove { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public Sprite(Texture2D tex, Vector2 pos, SpriteBatch spriteBatch)
        {
            Texture = tex;
            Position = pos;
            SpriteBatch = spriteBatch;
            Bounds = new Rectangle(
                (int)Position.X, (int)Position.Y,
                Texture.Width, Texture.Height
                );
            Remove = false;
        }

        public virtual void Draw()
        {
            SpriteBatch.Draw(Texture, Position - Map.Camera, Color.White);
        }

        public virtual void Update(GameTime gameTime)
        {
            Bounds = new Rectangle(
                (int)Position.X, (int)Position.Y,
                Texture.Width, Texture.Height
                );
        }

        public virtual void Interact(params object[] data)
        {
        }
    }
}
