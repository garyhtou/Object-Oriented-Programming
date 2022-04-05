using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GridFleaNS;

namespace GridFleaTests
{
    [TestClass]
    public class GridFleaUnitTests
    {
        // CONSTRUCTOR
        [TestMethod]
        public void Constructor_SetsEnergy()
        {
            int energy = 200;
            GridFlea g = new GridFlea(energy: energy);
            Assert.AreEqual(energy, g.GetEnergy(), "GridFlea energy not set by constructor");
        }

        [TestMethod]
        public void Constructor_ZeroEnergy_IsInactive()
        {
            GridFlea g = new GridFlea(energy: 0);
            Assert.IsTrue(g.IsInactive(), "GridFlea not inactive when zero energy");
        }

        [TestMethod]
        public void Constructor_NegativeEnergy_IsInactive()
        {
            GridFlea g = new GridFlea(energy: -10);
            Assert.IsTrue(g.IsInactive(), "GridFlea not inactive when negative energy");
        }

        [TestMethod]
        public void Constructor_PositiveEnergy_IsActive()
        {
            GridFlea g = new GridFlea(energy: 10);
            Assert.IsTrue(g.IsActive(), "GridFlea not active when positive energy");
        }

        [TestMethod]
        public void Constructor_SetsInitialX()
        {
            int initX = 15;
            GridFlea g = new GridFlea(x: initX);
            Assert.AreEqual(initX, g.GetX(), "GridFlea x position not set by constructor");
        }

        [TestMethod]
        public void Constructor_SetsInitialY()
        {
            int initY = 15;
            GridFlea g = new GridFlea(y: initY);
            Assert.AreEqual(initY, g.GetY(), "GridFlea y position not set by constructor");
        }

        [TestMethod]
        public void Constructor_SetsSize()
        {
            uint size = 200;
            GridFlea g = new GridFlea(size: size);
            Assert.AreEqual(size, g.GetSize(), "GridFlea size not set by constructor");
        }

        [TestMethod]
        public void Constructor_IsActive()
        {
            GridFlea g = new GridFlea();
            Assert.IsTrue(g.IsActive(), "default GridFlea is not active");
        }

        // RESET
        [TestMethod]
        public void Reset_SetsEnergy()
        {
            int initEnergy = 200;
            GridFlea g = new GridFlea(energy: initEnergy);
            g.Move(1); // Change energy
            g.Reset();
            Assert.AreEqual(initEnergy, g.GetEnergy(), "GridFlea energy not set to initial");
        }

        [TestMethod]
        public void Reset_SetsX()
        {
            int initX = 20;
            GridFlea g = new GridFlea(x: initX);
            g.Move(1);
            g.Move(1);
            g.Reset();
            Assert.AreEqual(initX, g.GetX(), "GridFlea x position not set to initial");
        }

        [TestMethod]
        public void Reset_SetsY()
        {
            int initY = 20;
            GridFlea g = new GridFlea(y: initY);
            g.Move(1);
            g.Move(1);
            g.Reset();
            Assert.AreEqual(initY, g.GetY(), "GridFlea y position not set to initial");
        }

        [TestMethod]
        public void Reset_SetsReward()
        {
            int initReward = 20;
            GridFlea g = new GridFlea(reward: initReward);
            g.Move(1);
            g.Reset();
            Assert.AreEqual(initReward, g.GetReward(), "GridFlea reward not set to initial");
        }

        [TestMethod]
        public void Reset_SetsDirection()
        {
            GridFlea g = new GridFlea();

            Axis initDirection = g.GetDirection();
            g.Move(1);
            g.Reset();
            Assert.AreEqual(initDirection, g.GetDirection(), "GridFlea direction not set to initial");
        }

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
        public void Revive_SetsEnergy()
        {
            GridFlea g = InactiveGridFleaFactory();

            int newEnergy = 100;
            g.Revive((uint)newEnergy);

            Assert.AreEqual(newEnergy, g.GetEnergy(), "GridFlea's new energy not set by revive");
        }

        [TestMethod]
        public void Revive_SetsActive()
        {
            GridFlea g = InactiveGridFleaFactory();

            g.Revive(100);

            Assert.IsTrue(g.IsActive(), "GridFlea's not active after revive");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Revive_WithActive_ThrowsError()
        {
            GridFlea g = new GridFlea();

            g.Revive(10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Revive_WithDead_ThrowsError()
        {
            GridFlea g = DeadGridFleaFactory();

            g.Revive(10);
        }

        // MOVE
        [TestMethod]
        public void Move_WithActive_MovesP()
        {
            int initX = 0;
            int initY = 0;
            GridFlea g = new GridFlea(x: initX, y: initY, energy: 10);

            int moveAmount = 10;

            Axis direction = g.GetDirection();
            g.Move(moveAmount);

            int movedAmount;
            if (direction == Axis.X)
            {
                movedAmount = g.GetX() - initX;
            }
            else
            {
                movedAmount = g.GetY() - initY;
            }

            Assert.AreEqual(moveAmount, movedAmount, "Active GridFlea did not move P squares");
        }

        [TestMethod]
        public void Move_WithInactive_Move1()
        {
            int initX = 0;
            int initY = 0;
            GridFlea g = InactiveGridFleaFactory();

            int moveAmount = 10;

            Axis direction = g.GetDirection();
            g.Move(moveAmount);

            int movedAmount;
            if (direction == Axis.X)
            {
                movedAmount = g.GetX() - initX;
            }
            else
            {
                movedAmount = g.GetY() - initY;
            }

            Assert.AreEqual(1, movedAmount, "Inactive GridFlea did not move 1 square");
        }

        [TestMethod]
        public void Move_WithNegative_GoesBackwards()
        {
            int initX = 0;
            int initY = 0;
            GridFlea g = new GridFlea(x: initX, y: initY, energy: 10);

            int moveAmount = -10;

            Axis direction = g.GetDirection();
            g.Move(moveAmount);

            int movedAmount;
            if (direction == Axis.X)
            {
                movedAmount = g.GetX() - initX;
            }
            else
            {
                movedAmount = g.GetY() - initY;
            }

            Assert.AreEqual(moveAmount, movedAmount, "GridFlea moved with negative square did not go backwards");
        }

        [TestMethod]
        public void Move_ChangesDirection()
        {
            GridFlea g = new GridFlea();

            Axis initDirection = g.GetDirection();
            g.Move(1);

            Assert.AreNotEqual(initDirection, g.GetDirection(), "GridFlea did not change directions after moving");
        }

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

        [TestMethod]
        public void Move_TilNegativeOutOfBounds_IsDead()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            Axis direction = g.GetDirection();
            int moveAmount = direction == Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;
            moveAmount++;
            moveAmount *= -1;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsDead(), "GridFlea past negative bound is not dead");
        }

        [TestMethod]
        public void Move_TilOnBounds_IsActive()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            Axis direction = g.GetDirection();
            int moveAmount = direction == Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsActive(), "GridFlea on border is not active");
        }

        [TestMethod]
        public void Move_TilOnNegativeBounds_IsActive()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            Axis direction = g.GetDirection();
            int moveAmount = direction == Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;
            moveAmount *= -1;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsActive(), "GridFlea on negative bound is not active");
        }

        [TestMethod]
        public void Move_UpdatesReward()
        {
            int initReward = 10;
            GridFlea g = new GridFlea(reward: initReward);
            int moveAmount = 3;
            g.Move(moveAmount);

            int expectedReward = initReward - moveAmount;
            Assert.AreEqual(expectedReward, g.GetReward(), "GridFlea move does not update reward");
        }

        [TestMethod]
        public void Move_DecrementEnergy()
        {
            int initEnergy = 10;
            GridFlea g = new GridFlea(energy: initEnergy);
            int moveAmount = 3;
            g.Move(moveAmount);

            int expectedEnergy = initEnergy - 1;
            Assert.AreEqual(expectedEnergy, g.GetEnergy(), "GridFlea move does not decrement energy");
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
        public void Value_BeforeMove()
        {
            int initReward = 15;
            uint size = 20;

            GridFlea g = new GridFlea(size: size, reward: initReward);

            int expectedChange = 0;
            int expectedValue = (int)(g.GetReward() * g.GetSize() * expectedChange);
            Assert.AreEqual(expectedValue, g.Value(), "GridFlea value not correct before move");
        }

        [TestMethod]
        public void Value_AfterMove()
        {
            int initReward = 15;
            uint size = 20;
            int initX = 50;
            int initY = -25;

            GridFlea g = new GridFlea(x: initX, y: initY, size: size, reward: initReward);
            int moveAmount = 3;
            g.Move(moveAmount);

            int expectedChange = Math.Abs(initX - g.GetX()) + Math.Abs(initY - g.GetY());
            int expectedValue = (int)(g.GetReward() * g.GetSize() * expectedChange);
            Assert.AreEqual(expectedValue, g.Value(), "GridFlea value not correct after move");
        }

        [TestMethod]
        public void Value_AfterMultipleMoves_IsNegative()
        {
            int initReward = 15;
            uint size = 20;
            int initX = 50;
            int initY = -25;

            GridFlea g = new GridFlea(x: initX, y: initY, size: size, reward: initReward);
            int moveAmount1 = 3;
            g.Move(moveAmount1);

            int moveAmount2 = 20;
            g.Move(moveAmount2);

            int expectedChange = Math.Abs(initX - g.GetX()) + Math.Abs(initY - g.GetY());
            int expectedValue = (int)(g.GetReward() * g.GetSize() * expectedChange);
            Assert.AreEqual(expectedValue, g.Value(), "GridFlea value not correct after multiple moves");
        }

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

            Axis direction = g.GetDirection();
            int moveAmount = direction == Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;
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
