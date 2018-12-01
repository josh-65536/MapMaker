using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MapMaker.DocumentModel;

namespace MapMaker
{
    public class LevelView : Control
    {
        private readonly LevelDocument _document;

        private int _tileX;
        private int _tileY;
        private bool _isInBounds;

        private Bitmap _framebuffer;
        private FastBitmap _compositeOutput;

        public LevelView(LevelDocument document)
        {
            _document = document;
            document.ChangeMap += (o, e) => Invalidate();

            MouseEnter += TileMode_MouseEnter;
            MouseLeave += TileMode_MouseLeave;
            MouseDown += TileMode_MouseDown;
            MouseMove += TileMode_MouseMove;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaint(e);

            var level = _document.Medium;
            var levelWidth = level.Width;
            var levelHeight = level.Height;
            var tileImages = level.Tileset.Tiles;
            var tileWidth = level.Tileset.TileWidth;
            var tileHeight = level.Tileset.TileHeight;

            var g = e.Graphics;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighSpeed;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.HighSpeed;

            if (_framebuffer == null || _framebuffer.Width != ClientSize.Width || _framebuffer.Height != ClientSize.Height)
            {
                _framebuffer?.Dispose();
                _framebuffer = new Bitmap(ClientSize.Width, ClientSize.Height, g);
                _compositeOutput = new FastBitmap(ClientSize.Width, ClientSize.Height);
            }

            for (var i = 0; i < levelHeight; ++i)
            {
                var dy = i * tileHeight;

                for (var j = 0; j < levelWidth; ++j)
                {
                    var dx = j * tileWidth;
                    var tile = level.GetTile(j, i);

                    FastBitmap.Blit(tileImages[tile], _compositeOutput, dx, dy);
                }
            }

            var data = _framebuffer.LockBits(
                new Rectangle(0, 0, _framebuffer.Width, _framebuffer.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb
            );
            for (var i = 0; i < _compositeOutput.Height; ++i)
            {
                Marshal.Copy(
                    _compositeOutput.Pixels,
                    i * _compositeOutput.Width,
                    IntPtr.Add(data.Scan0, i * data.Stride),
                    _compositeOutput.Width
                );
            }
            _framebuffer.UnlockBits(data);

            g.DrawImageUnscaled(_framebuffer, 0, 0);

            var warpBrush = new SolidBrush(Color.FromArgb(127, 0, 255, 0));
            var selectionBrush = new SolidBrush(Color.FromArgb(127, 255, 255, 255));

            foreach (var w in level.Warps)
            {
                var x = w.X * tileWidth;
                var y = w.Y * tileHeight;
                var width = w.Width * tileWidth;
                var height = w.Height * tileHeight;

                g.FillRectangle(warpBrush, x, y, width, height);
            }

            if (_isInBounds)
            {
                var rectX = _tileX * tileWidth;
                var rectY = _tileY * tileHeight;
                g.FillRectangle(selectionBrush, rectX, rectY, tileWidth, tileHeight);
            }

            selectionBrush.Dispose();
            warpBrush.Dispose();
        }

        private int CursorToTileX(int cursorX) => cursorX / _document.Medium.Tileset.TileWidth;

        private int CursorToTileY(int cursorY) => cursorY / _document.Medium.Tileset.TileHeight;

        private void TileMode_MouseEnter(object sender, EventArgs e)
        {
            _isInBounds = true;
        }

        private void TileMode_MouseLeave(object sender, EventArgs e)
        {
            _isInBounds = false;
            Invalidate();
        }

        private void TileMode_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _document.Medium.SetTile(_tileX, _tileY, 100);
                Invalidate();
            }
        }

        private void TileMode_MouseMove(object sender, MouseEventArgs e)
        {
            _tileX = CursorToTileX(e.X);
            _tileY = CursorToTileY(e.Y);

            if (e.Button == MouseButtons.Left)
                _document.Medium.SetTile(_tileX, _tileY, 100);

            Invalidate();
        }
    }
}