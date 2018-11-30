using System;

namespace MapMaker.DocumentModel
{
    public class Document
    {
        public Level Level { get; }

        public event EventHandler<LevelEventArgs> ChangeMap;
        public event EventHandler<EditModeEventArgs> ChangeEditMode;

        private EditMode _editMode;

        public Warp SelectedWarp { get; private set; }

        public Document(Level level)
        {
            Level = level;

            ChangeMap += (o, e) => { };
            ChangeEditMode += (o, e) => { };

            _editMode = EditMode.Tile;
        }

        public EditMode EditMode
        {
            get => _editMode;
            set
            {
                _editMode = value;
                ChangeEditMode(this, new EditModeEventArgs(_editMode));
            }
        }

        public void SelectWarp(int x, int y)
        {
            SelectedWarp = Level.GetWarp(x, y);
        }

        public void NotifyMapChange() => ChangeMap(this, new LevelEventArgs(Level));

        public void NotifyEditModeChange() => ChangeEditMode(this, new EditModeEventArgs(_editMode));
    }
}