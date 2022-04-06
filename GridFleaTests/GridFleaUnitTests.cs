// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GridFleaNS;

namespace GridFleaTests
{
    [TestClass]
    public class GridFleaUnitTests
    {
        [TestMethod]
        public void Constructor_ZeroEnergy_IsInactive()
        {
            GridFlea g = new GridFlea(energy: 0);
            Assert.IsTrue(g.IsInactive(), "GridFlea not inactive when zero energy");
        }

        [TestMethod]
        public void Constructor_NegativeEnergy_IsInactive()
        {
            const int ENERGY = -10;
            GridFlea g = new GridFlea(energy: ENERGY);
            Assert.IsTrue(g.IsInactive(), "GridFlea not inactive when negative energy");
        }

        [TestMethod]
        public void Constructor_PositiveEnergy_IsActive()
        {
            const int ENERGY = 10;
            GridFlea g = new GridFlea(energy: ENERGY);
            Assert.IsTrue(g.IsActive(), "GridFlea not active when positive energy");
        }

        [TestMethod]
        public void Constructor_IsActive()
        {
            GridFlea g = new GridFlea();
            Assert.IsTrue(g.IsActive(), "default GridFlea is not active");
        }

        // RESET
        [TestMethod]
        public void Reset_WithInactive_SetsActive()
        {
            GridFlea g = InactiveGridFleaFactory();
            g.Reset();
            Assert.IsTrue(g.IsActive(), "GridFlea not active after reset");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Reset_WithDead_ThrowsError()
        {
            GridFlea g = DeadGridFleaFactory();

            g.Reset();
        }

        // REVIVE
        [TestMethod]
        public void Revive_SetsActive()
        {
            GridFlea g = InactiveGridFleaFactory();

            const int NEW_ENERGY = 100;
            g.Revive(NEW_ENERGY);

            Assert.IsTrue(g.IsActive(), "GridFlea's not active after revive");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Revive_WithActive_ThrowsError()
        {
            GridFlea g = new GridFlea();

            const int NEW_ENERGY = 10;
            g.Revive(NEW_ENERGY);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Revive_WithDead_ThrowsError()
        {
            GridFlea g = DeadGridFleaFactory();

            const int NEW_ENERGY = 10;
            g.Revive(NEW_ENERGY);
        }

        // MOVE
        [TestMethod]
        public void Move_TilNoEnergy_IsInactive()
        {
            GridFlea g = InactiveGridFleaFactory();

            Assert.IsTrue(g.IsInactive(), "GridFlea from DeadGridFleaFactory (Move til no energy) is not inactive");
        }

        [TestMethod]
        public void Move_TilOutOfBounds_IsDead()
        {
            GridFlea g = DeadGridFleaFactory();

            Assert.IsTrue(g.IsDead(), "GridFlea from DeadGridFleaFactory (Move til out of bounds) is not dead");
        }

        const int GRID_BOUND = 1_000;
        [TestMethod]
        public void Move_TilNegativeOutOfBounds_IsDead()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            int moveAmount = GRID_BOUND;
            moveAmount++;
            moveAmount *= -1;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsDead(), "GridFlea past negative bound is not dead");
        }

        [TestMethod]
        public void Move_TilOnBounds_IsActive()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            g.Move(GRID_BOUND);

            Assert.IsTrue(g.IsActive(), "GridFlea on border is not active");
        }

        [TestMethod]
        public void Move_TilOnNegativeBounds_IsActive()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            int moveAmount = GRID_BOUND;
            moveAmount *= -1;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsActive(), "GridFlea on negative bound is not active");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Move_WithDead_ThrowsError()
        {
            GridFlea g = DeadGridFleaFactory();
            g.Move(1);
        }

        // VALUE
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Value_WithInactive_ThrowsError()
        {
            GridFlea g = InactiveGridFleaFactory();
            g.Value();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Value_WithDead_ThrowsError()
        {
            GridFlea g = DeadGridFleaFactory();
            g.Value();
        }


        // FACTORIES
        private GridFlea InactiveGridFleaFactory()
        {
            GridFlea g = new GridFlea(energy: 1);

            g.Move(1);
            return g;
        }
        [TestMethod]
        public void InactiveGridFleaFactory_IsInactive()
        {
            GridFlea g = InactiveGridFleaFactory();
            Assert.IsTrue(g.IsInactive());
        }

        private GridFlea DeadGridFleaFactory()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            int moveAmount = GRID_BOUND;
            moveAmount++;

            g.Move(moveAmount); // Move out of bounds (become dead)

            return g;
        }
        [TestMethod]
        public void DeadGridFleaFactory_IsDead()
        {
            GridFlea g = DeadGridFleaFactory();
            Assert.IsTrue(g.IsDead());
        }
    }
}
