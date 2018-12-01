using MapMaker.DocumentModel;
using Microsoft.VisualBasic;
using System;
using System.Windows.Forms;

namespace MapMaker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var tilesetFilePath = Interaction.InputBox("Enter the tileset file:");

            var level = new Level(64, 64, Tileset.Load(tilesetFilePath));
            var document = new LevelDocument(level);

            Application.Run(new MainWindow(document));
        }
    }
}