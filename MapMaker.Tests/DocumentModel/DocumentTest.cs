using System;
using MapMaker.DocumentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapMaker.Tests.DocumentModel
{
    [TestClass]
    public class DocumentTest
    {
        [TestMethod]
        public void Undo_IntValue()
        {
            var document = new SampleDocument();
            var view = (Revision<int>.View) document.Medium.IntValue;

            view.CheckIn(1000);
            view.CheckIn(2000);
            view.CheckIn(3000);
            view.CheckIn(4000);
            view.CheckIn(5000);

            Assert.AreEqual(5000, view);

            document.Undo();
            Assert.AreEqual(4000, view);

            document.Undo();
            Assert.AreEqual(3000, view);

            document.Undo();
            Assert.AreEqual(2000, view);

            document.Undo();
            Assert.AreEqual(1000, view);

            document.Undo();
            Assert.AreEqual(0, view);
        }
    }
}