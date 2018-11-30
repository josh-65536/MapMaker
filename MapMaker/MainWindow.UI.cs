using MapMaker.DocumentModel;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MapMaker
{
    public partial class MainWindow : Form
    {
        private SplitContainer _mainSplitContainer;
        private LevelView _levelView;

        private MenuItem _menuFileNew;
        private MenuItem _menuFileOpen;
        private MenuItem _menuFileSave;
        private MenuItem _menuFileSaveAs;
        private MenuItem _menuFileSeparator1;
        private MenuItem _menuFileExit;
        private MenuItem _menuFile;
        private MenuItem _menuEditUndo;
        private MenuItem _menuEditRedo;
        private MenuItem _menuEdit;
        private MenuItem _menuModeTileMode;
        private MenuItem _menuModeWarpMode;
        private MenuItem _menuMode;
        private MenuItem _menuAreaSetMusic;
        private MenuItem _menuArea;
        private MenuItem _menuViewZoom200;
        private MenuItem _menuView;
        private MenuItem _menuHelpAboutMapMaker;
        private MenuItem _menuHelp;
        private MainMenu _mainMenu;

        private void InitializeComponent(Document document)
        {
            _mainSplitContainer = new SplitContainer();
            _mainSplitContainer.BeginInit();
            _mainSplitContainer.SuspendLayout();
            SuspendLayout();

            _levelView = new LevelView(document);
            _levelView.Dock = DockStyle.Fill;

            _menuFileNew = new MenuItem("&New", OnFileNewClick, Shortcut.CtrlN);
            _menuFileOpen = new MenuItem("&Open", OnFileOpenClick, Shortcut.CtrlO);
            _menuFileSave = new MenuItem("&Save", OnFileSaveClick, Shortcut.CtrlS);
            _menuFileSaveAs = new MenuItem("Save As...", OnFileSaveAsClick);
            _menuFileSeparator1 = new MenuItem("-");
            _menuFileExit = new MenuItem("E&xit", OnFileExitClick, Shortcut.AltF4);
            _menuFile = new MenuItem("&File", new[]
            {
                _menuFileNew,
                _menuFileOpen,
                _menuFileSave,
                _menuFileSaveAs,
                _menuFileSeparator1,
                _menuFileExit
            });
            _menuEditUndo = new MenuItem("&Undo", OnEditUndoClick, Shortcut.CtrlZ);
            _menuEditRedo = new MenuItem("&Redo", OnEditRedoClick, Shortcut.CtrlY);
            _menuEdit = new MenuItem("&Edit", new[]
            {
                _menuEditUndo,
                _menuEditRedo
            });
            _menuModeTileMode = new MenuItem("&Tile Mode", OnModeTileModeClick);
            _menuModeWarpMode = new MenuItem("&Warp Mode", OnModeWarpModeClick);
            _menuMode = new MenuItem("&Mode", new[]
            {
                _menuModeTileMode,
                _menuModeWarpMode
            });
            _menuAreaSetMusic = new MenuItem("Set &Music", OnAreaSetMusicClick);
            _menuArea = new MenuItem("&Area", new[]
            {
                _menuAreaSetMusic
            });
            _menuViewZoom200 = new MenuItem("Zoom &200%", OnViewZoom200Click);
            _menuView = new MenuItem("&View", new[]
            {
                _menuViewZoom200
            });
            _menuHelpAboutMapMaker = new MenuItem("&About Map Maker", OnHelpAboutMapMaker);
            _menuHelp = new MenuItem("&Help", new[]
            {
                _menuHelpAboutMapMaker
            });

            _mainMenu = new MainMenu(new[]
            {
                _menuFile,
                _menuEdit,
                _menuMode,
                _menuArea,
                _menuView,
                _menuHelp
            });

            _mainSplitContainer.Dock = DockStyle.Fill;
            _mainSplitContainer.Location = new Point();
            _mainSplitContainer.Size = new Size(800, 450);
            _mainSplitContainer.SplitterDistance = 500;

            _mainSplitContainer.Panel1.Controls.Add(_levelView);

            AutoScaleDimensions = new SizeF(8.0F, 16.0F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Text = "Map Maker";

            Controls.Add(_mainSplitContainer);
            Menu = _mainMenu;

            _mainSplitContainer.EndInit();
            _mainSplitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        partial void OnFileNewClick(object sender, EventArgs e);
        partial void OnFileOpenClick(object sender, EventArgs e);
        partial void OnFileSaveClick(object sender, EventArgs e);
        partial void OnFileSaveAsClick(object sender, EventArgs e);
        partial void OnFileExitClick(object sender, EventArgs e);
        partial void OnEditUndoClick(object sender, EventArgs e);
        partial void OnEditRedoClick(object sender, EventArgs e);
        partial void OnModeTileModeClick(object sender, EventArgs e);
        partial void OnModeWarpModeClick(object sender, EventArgs e);
        partial void OnAreaSetMusicClick(object sender, EventArgs e);
        partial void OnViewZoom200Click(object sender, EventArgs e);
        partial void OnHelpAboutMapMaker(object sender, EventArgs e);
    }
}