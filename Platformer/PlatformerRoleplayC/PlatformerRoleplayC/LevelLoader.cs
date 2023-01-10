using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerRoleplayC
{
    public class LevelLoader
    {
        public static int[,] TextToLevel(string[] text, out int width, out int height)
        {
            int[,] r = new int[P(text[0].Split('.')[0]), P(text[0].Split('.')[1])];

            for (int i = 1; i < text.Length; i++)
            {
                string row = text[i];
                for (int x = 0; x < row.Length; x++)
                {
                    r[x, i - 1] = P(row[x].ToString());
                }
            }

            width = P(text[0].Split('.')[0]);
            height = P(text[0].Split('.')[1]);

            return r;
        }

        public static void SetExtras(string[] text, Block[,] Blocks, Texture2D[] BlockTextures)
        {
            for (int i = 0; i < text.Length; i++)
            {
                int x = P(text[i].Split('.')[0]), y = P(text[i].Split('.')[1]), id = P(text[i].Split('.')[2]),
                    layer = P(text[i].Split('.')[3]);
                var b = Blocks[x, y];
                b.Texture = BlockTextures[id];
                b.ID = id;
                b.Layer = layer;
                if (id != 0)
                    b.Bounds = new Microsoft.Xna.Framework.Rectangle((int)b.Position.X, (int)b.Position.Y, b.Texture.Width, b.Texture.Height);
                if (id == 4)
                {
                    int teleporterId = P(text[i].Split('.')[4]), target = P(text[i].Split('.')[5]);
                    b.ExtraData = new object[2];
                    b.ExtraData[0] = teleporterId;
                    b.ExtraData[1] = target;
                }
            }
        }

        public static int P(string s) {
            return Convert.ToInt32(s); }
    }
}
