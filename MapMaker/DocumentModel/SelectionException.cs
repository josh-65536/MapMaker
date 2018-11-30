using System;

namespace MapMaker.DocumentModel
{
    public class SelectionException : Exception
    {
        public SelectionException() { }

        public SelectionException(string message) : base(message) { }
    }
}