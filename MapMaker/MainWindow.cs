using MapMaker.DocumentModel;
using System;

namespace MapMaker
{
    public partial class MainWindow
    {
        private readonly LevelDocument _document;

        public MainWindow(LevelDocument document)
        {
            InitializeComponent(document);

            _document = document;
        }

        partial void OnFileNewClick(object sender, EventArgs e) { }
        partial void OnFileOpenClick(object sender, EventArgs e) { }
        partial void OnFileSaveClick(object sender, EventArgs e) { }
        partial void OnFileSaveAsClick(object sender, EventArgs e) { }
        partial void OnFileExitClick(object sender, EventArgs e) { }
        partial void OnEditUndoClick(object sender, EventArgs e) { }
        partial void OnEditRedoClick(object sender, EventArgs e) { }
        partial void OnModeTileModeClick(object sender, EventArgs e) { }
        partial void OnModeWarpModeClick(object sender, EventArgs e) { }
        partial void OnAreaSetMusicClick(object sender, EventArgs e) { }
        partial void OnViewZoom200Click(object sender, EventArgs e) { }
        partial void OnHelpAboutMapMaker(object sender, EventArgs e) { }
    }
}