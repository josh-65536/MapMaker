using System;
using System.Collections.Generic;
using System.IO;
using MapMaker.Assets;

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
        private string MusicPath { get; set; }

        private readonly int[] _map;
        private readonly List<Warp> _warps;

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
            _warps = new List<Warp>();
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

        public Warp GetWarp(int x, int y)
        {
            foreach (var w in _warps)
            {
                if (x >= w.X && y >= w.Y && x < w.X + w.Width && y < w.Y + w.Height)
                    return w;
            }

            return null;
        }

        public IEnumerable<Warp> Warps => _warps;

        public Warp AddWarp(Warp warp)
        {
            _warps.Add(warp);
            return warp;
        }

        public Warp RemoveWarp(int x, int y)
        {
            for (var i = 0; i < _warps.Count; ++i)
            {
                var w = _warps[i];

                if (x >= w.X && y >= w.Y && x < w.X + w.Width && y < w.Y + w.Height)
                {
                    _warps.RemoveAt(i);
                    return w;
                }
            }

            return null;
        }

        public Warp RemoveWarp(Warp warp)
        {
            _warps.Remove(warp);
            return warp;
        }
    }
}