using System.Drawing;
using MapMaker.Assets;

namespace MapMaker.DocumentModel
{
    public class FastBitmap
    {
        public int Width { get; }
        public int Height { get; }
        public int[] Pixels { get; }

        public FastBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Pixels = new int[Width * Height];
        }

        private FastBitmap(int width, int height, int[] pixels)
        {
            Width = width;
            Height = height;
            Pixels = pixels;
        }

        public static FastBitmap FromFile(string filePath)
        {
            var loadedBitmap = (Bitmap) Image.FromStream(AssetLoader.LoadStream(filePath));
            var result = new FastBitmap(loadedBitmap.Width, loadedBitmap.Height);

            for (var i = 0; i < result.Height; ++i)
            {
                for (var j = 0; j < result.Width; ++j)
                {
                    var color = loadedBitmap.GetPixel(j, i);
                    result.Pixels[j + i * result.Width] = color.ToArgb();
                }
            }

            loadedBitmap.Dispose();
            return result;
        }

        public int GetPixel(int x, int y) => Pixels[x + y * Width];

        public void SetPixel(int x, int y, int to) => Pixels[x + y * Width] = to;

        public static void Blit(FastBitmap source, FastBitmap destination, int destX, int destY)
        {
            for (var y = 0; y < source.Height; ++y)
            {
                var blitY = destY + y;
                if (blitY < 0 || blitY >= destination.Height)
                    continue;

                for (var x = 0; x < source.Width; ++x)
                {
                    var blitX = destX + x;
                    if (blitX < 0 || blitX >= destination.Width)
                        continue;

                    var sourcePixel = source.Pixels[x + y * source.Width];
                    destination.Pixels[blitX + blitY * destination.Width] = sourcePixel;
                }
            }
        }
    }
}