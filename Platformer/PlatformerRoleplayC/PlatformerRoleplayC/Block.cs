using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerRoleplayC
{
    public class Block : Sprite
    {
        public int ID { get; set; }

        public int Delay { get; set; }
        public bool Shot { get; set; }
        public string Shooter { get; set; }

        public int Layer { get; set; }

        public object[] ExtraData { get; set; }

        public Block(Texture2D tex, Vector2 pos, SpriteBatch spriteBatch, int layer, int id) : base(tex, pos, spriteBatch)
        {
            Layer = layer;

            ID = id;

            Shot = false;

            Shooter = "0";

            Delay = 0;
        }

        public override void Draw()
        {
            if (Shot && Delay != 0)
            {
                Delay--;
                Dictionary<string, Color> colors = new Dictionary<string, Color>();
                colors.Add("1", Color.Blue);colors.Add("2", Color.Red);colors.Add("3", Color.Green);colors.Add("4", Color.Yellow);
                SpriteBatch.DrawString(Game1.font, Shooter,
                    new Vector2(Position.X-Map.Camera.X, Position.Y - Texture.Height-Map.Camera.Y), colors[Shooter]);
                if (Delay == 0)
                {
                    Shot = false;
                    Shooter = "0";
                }
            }

            base.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            if (ID != 0)
                base.Update(gameTime);
            else
                Bounds = new Rectangle(0, 0, 0, 0);
        }

        public override void Interact(params object[] data)
        {
            if ((string)data[0] == "shot")
            {
                Shot = true;
                Shooter = (data[1] as Player).ID.ToString();
                Delay = 100;
            }
            base.Interact(data);
        }
    }
}
