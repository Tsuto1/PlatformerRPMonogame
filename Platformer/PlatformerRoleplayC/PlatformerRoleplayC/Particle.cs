using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerRoleplayC
{
    public class Particle : Sprite
    {
        public Vector2 Movement { get; set; }

        public int Life { get; set; }

        public Particle(Texture2D tex, Vector2 pos, SpriteBatch spriteBatch,
            Vector2 movement, int life, Color color, Point size) : base(tex, pos, spriteBatch)
        {
            Movement = movement;
            Life = life;
        }

        public override void Update(GameTime gameTime)
        {
            Movement /= 2;
            Position += Movement;
            Life--;
            if (Life == 0)
                Remove = true;
            base.Update(gameTime);
        }
    }
}
