using System;

namespace MapMaker.DocumentModel
{
    public class Level
    {
        public const int FlipX = 0x4000;
        public const int FlipY = 0x8000;
        public const int TileMask = ~(FlipX | FlipY);

        public int Width { get; }
        public int Height { get; }
        public Tileset Tileset { get; }

        private readonly int[] _map;

        public Level(int width, int height, Tileset tileset)
        {
            if (width <= 0)
                throw new ArgumentException(nameof(width));

            if (height <= 0)
                throw new ArgumentException(nameof(height));

            Width = width;
            Height = height;
            Tileset = tileset;

            _map = new int[width * height];
        }

        public int GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return 0;

            return _map[x + y * Width];
        }

        public void SetTile(int x, int y, int to)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
                _map[x + y * Width] = to;
        }
    }
}