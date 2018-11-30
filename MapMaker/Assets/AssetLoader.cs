using System.IO;
using Microsoft.VisualBasic;

namespace MapMaker.Assets
{
    public static class AssetLoader
    {
        private static readonly string _rootPath = "";

        static AssetLoader()
        {
            _rootPath = Interaction.InputBox(
                "Enter the asset directory:",
                DefaultResponse: _rootPath
            );
        }

        public static Stream LoadStream(string filePath) => File.OpenRead(Path.Combine(_rootPath, filePath));

        public static string LoadText(string filePath) => File.ReadAllText(Path.Combine(_rootPath, filePath));
    }
}