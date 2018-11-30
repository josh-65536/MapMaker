using System;

namespace MapMaker.DocumentModel
{
    public class EditModeEventArgs : EventArgs
    {
        public EditMode NewMode { get; }

        public EditModeEventArgs(EditMode newMode)
        {
            NewMode = newMode;
        }
    }
}