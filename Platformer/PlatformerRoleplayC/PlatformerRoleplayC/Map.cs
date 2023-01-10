using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformerRoleplayC
{
    public class Map
    {
        public static Map Current { get; private set; }

        public static Vector2 Camera { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public static Texture2D[] SpriteTextures { get; set; }

        public List<Sprite> Sprites { get; set; }

        public Block[,] Blocks { get; set; }

        public Player[] Players { get; set; }

        public Texture2D[] BlockTextures { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public Map(int width, int height, Texture2D[] blockTextures, SpriteBatch spriteBatch)
        {
            Sprites = new List<Sprite>();
            Width = width;
            Height = height;
            SpriteBatch = spriteBatch;
            BlockTextures = blockTextures;
            Blocks = new Block[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    Blocks[x, y] = new Block(
                        blockTextures[0],
                        new Vector2(
                            x * blockTextures[0].Width,
                            y * blockTextures[0].Height),
                        spriteBatch, 0, 0);
                    Blocks[x, y].Bounds = new Rectangle(0, 0, 0, 0);
                    if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                        Blocks[x, y] = new Block(
                        blockTextures[1],
                        new Vector2(
                            x * blockTextures[1].Width,
                            y * blockTextures[1].Height),
                        spriteBatch, 0, 1);
                }
            Camera = Vector2.Zero;
            Current = this;
        }
        
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Sprites.Count; i++)
                if (Sprites[i].Remove)
                    Sprites.Remove(Sprites[i]);
            Current = this;
            foreach (var b in Blocks)
                b.Update(gameTime);
            foreach (var s in Sprites)
                s.Update(gameTime);
        }
        
        public void Draw()
        {
            foreach (var block in Blocks)
                if (block.ID != 0)
                    block.Draw();
            foreach (var s in Sprites)
                s.Draw();
        }

        public void Load(string filename, string fileextras = null)
        {
            int width, height;
            var ids = LevelLoader.TextToLevel(File.ReadAllLines(filename), out width, out height);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    if (ids[x, y] == 4|| ids[x, y] == 2)
                        Set(x, y, ids[x, y], 1);
                    else
                    {
                        Set(x, y, ids[x, y], 0);
                    }
                }
            if (fileextras != null)
                LevelLoader.SetExtras(File.ReadAllLines(fileextras), Blocks, BlockTextures);
        }

        public void Set(int x, int y, int id, int layer, object[] extra = null)
        {
            var b = Blocks[x, y];
            b.Texture = BlockTextures[id];
            b.ID = id;
            b.Layer = layer;
            if (id != 0)
                b.Bounds = new Rectangle((int)b.Position.X, (int)b.Position.Y, b.Texture.Width, b.Texture.Height);
        }
    }
}
