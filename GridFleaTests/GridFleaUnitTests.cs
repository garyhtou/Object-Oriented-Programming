using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GridFleaNS;

namespace GridFleaTests
{
    [TestClass]
    public class GridFleaUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            GridFlea g = new GridFlea();
            g.GetState();
        }
    }
}
