using System.Drawing;
using MapMaker.Assets;

namespace MapMaker.DocumentModel
{
    public class Tileset
    {
        public string FilePath { get; }
        public FastBitmap[] Tiles { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public int TileCountX { get; }
        public int TileCountY { get; }

        private Tileset(string filePath, Tag tag)
        {
            FilePath = filePath;
            TileWidth = tag["tileWidth"].IntValue;
            TileHeight = tag["tileHeight"].IntValue;

            var bitmap = (Bitmap) Image.FromStream(AssetLoader.LoadStream(tag["texture"].StringValue));
            TileCountX = bitmap.Width / TileWidth;
            TileCountY = bitmap.Height / TileHeight;

            var tileCount = TileCountX * TileCountY;
            var tileImages = new FastBitmap[tileCount];

            for (var i = 0; i < tileCount; ++i)
            {
                var tileX = i % TileCountX;
                var tileY = i / TileCountY;

                tileImages[i] = new FastBitmap(TileWidth, TileHeight);

                for (var j = 0; j < TileHeight; ++j)
                {
                    for (var k = 0; k < TileWidth; ++k)
                    {
                        var sourceX = k + (tileX * TileWidth);
                        var sourceY = j + (tileY * TileHeight);
                        var pixel = bitmap.GetPixel(sourceX, sourceY);

                        tileImages[i].SetPixel(k, j, pixel.ToArgb());
                    }
                }
            }

            bitmap.Dispose();

            Tiles = tileImages;
        }

        public static Tileset Load(string filePath) => new Tileset(filePath, Tag.Load(filePath));

        public int GetTile(int x, int y) => (x / TileWidth) + (y / TileHeight) * TileCountX;

        public int GetTileX(int tile) => tile % TileCountX;

        public int GetTileY(int tile) => tile / TileCountX;

        public int GetSourceX(int tile) => (tile % TileCountX) * TileWidth;

        public int GetSourceY(int tile) => (tile / TileCountX) * TileWidth;
    }
}