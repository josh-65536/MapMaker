
namespace MapMaker.DocumentModel
{
    public class Warp
    {
        public const byte Entrance = 0;
        public const byte Exit = 1;

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ExitX { get; set; }
        public int ExitY { get; set; }
        public string Destination { get; set; }

        public Warp(string destination)
        {
            Destination = destination;
            Width = 1;
            Height = 1;
        }
    }
}