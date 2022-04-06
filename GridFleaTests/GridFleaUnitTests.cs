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
        // CONSTRUCTOR
        [TestMethod]
        public void Constructor_SetsEnergy()
        {
            const int ENERGY = 200;
            GridFlea g = new GridFlea(energy: ENERGY);
            Assert.AreEqual(ENERGY, g.GetEnergy(), "GridFlea energy not set by constructor");
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
        public void Constructor_SetsInitialX()
        {
            const int INIT_X = 15;
            GridFlea g = new GridFlea(x: INIT_X);
            Assert.AreEqual(INIT_X, g.GetX(), "GridFlea x position not set by constructor");
        }

        [TestMethod]
        public void Constructor_SetsInitialY()
        {
            const int INIT_Y = 15;
            GridFlea g = new GridFlea(y: INIT_Y);
            Assert.AreEqual(INIT_Y, g.GetY(), "GridFlea y position not set by constructor");
        }

        [TestMethod]
        public void Constructor_SetsSize()
        {
            const uint SIZE = 200;
            GridFlea g = new GridFlea(size: SIZE);
            Assert.AreEqual(SIZE, g.GetSize(), "GridFlea size not set by constructor");
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
            const int INIT_ENERGY = 200;
            GridFlea g = new GridFlea(energy: INIT_ENERGY);
            g.Move(1); // Change energy
            g.Reset();
            Assert.AreEqual(INIT_ENERGY, g.GetEnergy(), "GridFlea energy not set to initial");
        }

        [TestMethod]
        public void Reset_SetsX()
        {
            const int INIT_X = 20;
            GridFlea g = new GridFlea(x: INIT_X);
            g.Move(1);
            g.Move(1);
            g.Reset();
            Assert.AreEqual(INIT_X, g.GetX(), "GridFlea x position not set to initial");
        }

        [TestMethod]
        public void Reset_SetsY()
        {
            const int INIT_Y = 20;
            GridFlea g = new GridFlea(y: INIT_Y);
            g.Move(1);
            g.Move(1);
            g.Reset();
            Assert.AreEqual(INIT_Y, g.GetY(), "GridFlea y position not set to initial");
        }

        [TestMethod]
        public void Reset_SetsReward()
        {
            const int INIT_REWARD = 20;
            GridFlea g = new GridFlea(reward: INIT_REWARD);
            g.Move(1);
            g.Reset();
            Assert.AreEqual(INIT_REWARD, g.GetReward(), "GridFlea reward not set to initial");
        }

        [TestMethod]
        public void Reset_SetsDirection()
        {
            GridFlea g = new GridFlea();

            GridFlea.Axis initDirection = g.GetDirection();
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

            const int NEW_ENERGY = 100;
            g.Revive(NEW_ENERGY);

            Assert.AreEqual(NEW_ENERGY, g.GetEnergy(), "GridFlea's new energy not set by revive");
        }

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
        public void Move_WithActive_MovesP()
        {
            const int INIT_X = 0;
            const int INIT_Y = 0;
            const int INIT_ENERGY = 0;
            GridFlea g = new GridFlea(x: INIT_X, y: INIT_Y, energy: INIT_ENERGY);

            const int MOVE_AMOUNT = 10;

            GridFlea.Axis direction = g.GetDirection();
            g.Move(MOVE_AMOUNT);

            int movedAmount;
            if (direction == GridFlea.Axis.X)
            {
                movedAmount = g.GetX() - INIT_X;
            }
            else
            {
                movedAmount = g.GetY() - INIT_Y;
            }

            Assert.AreEqual(MOVE_AMOUNT, movedAmount, "Active GridFlea did not move P squares");
        }

        [TestMethod]
        public void Move_WithInactive_Move1()
        {
            const int INIT_X = 0;
            const int INIT_Y = 0;
            GridFlea g = InactiveGridFleaFactory();

            const int MOVE_AMOUNT = 10;

            GridFlea.Axis direction = g.GetDirection();
            g.Move(MOVE_AMOUNT);

            int movedAmount;
            if (direction == GridFlea.Axis.X)
            {
                movedAmount = g.GetX() - INIT_X;
            }
            else
            {
                movedAmount = g.GetY() - INIT_Y;
            }

            const int EXPECTED = 1;
            Assert.AreEqual(EXPECTED, movedAmount, "Inactive GridFlea did not move 1 square");
        }

        [TestMethod]
        public void Move_WithNegative_GoesBackwards()
        {
            const int INIT_X = 0;
            const int INIT_Y = 0;
            const int INIT_ENERGY = 10;
            GridFlea g = new GridFlea(x: INIT_X, y: INIT_Y, energy: INIT_ENERGY);

            const int MOVE_AMOUNT = -10;

            GridFlea.Axis direction = g.GetDirection();
            g.Move(MOVE_AMOUNT);

            int movedAmount;
            if (direction == GridFlea.Axis.X)
            {
                movedAmount = g.GetX() - INIT_X;
            }
            else
            {
                movedAmount = g.GetY() - INIT_Y;
            }

            Assert.AreEqual(MOVE_AMOUNT, movedAmount, "GridFlea moved with negative square did not go backwards");
        }

        [TestMethod]
        public void Move_ChangesDirection()
        {
            GridFlea g = new GridFlea();

            GridFlea.Axis initDirection = g.GetDirection();
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

            GridFlea.Axis direction = g.GetDirection();
            int moveAmount = direction == GridFlea.Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;
            moveAmount++;
            moveAmount *= -1;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsDead(), "GridFlea past negative bound is not dead");
        }

        [TestMethod]
        public void Move_TilOnBounds_IsActive()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            GridFlea.Axis direction = g.GetDirection();
            int moveAmount = direction == GridFlea.Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsActive(), "GridFlea on border is not active");
        }

        [TestMethod]
        public void Move_TilOnNegativeBounds_IsActive()
        {
            GridFlea g = new GridFlea(x: 0, y: 0);

            GridFlea.Axis direction = g.GetDirection();
            int moveAmount = direction == GridFlea.Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;
            moveAmount *= -1;

            g.Move(moveAmount);

            Assert.IsTrue(g.IsActive(), "GridFlea on negative bound is not active");
        }

        [TestMethod]
        public void Move_UpdatesReward()
        {
            const int INIT_REWARD = 10;
            GridFlea g = new GridFlea(reward: INIT_REWARD);
            const int MOVE_AMOUNT = 3;
            g.Move(MOVE_AMOUNT);

            int expectedReward = INIT_REWARD - MOVE_AMOUNT;
            Assert.AreEqual(expectedReward, g.GetReward(), "GridFlea move does not update reward");
        }

        [TestMethod]
        public void Move_DecrementEnergy()
        {
            const int INIT_ENERGY = 10;
            GridFlea g = new GridFlea(energy: INIT_ENERGY);
            const int MOVE_AMOUNT = 3;
            g.Move(MOVE_AMOUNT);

            const int EXPECTED = INIT_ENERGY - 1;
            Assert.AreEqual(EXPECTED, g.GetEnergy(), "GridFlea move does not decrement energy");
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
            const int INIT_REWARD = 15;
            const uint SIZE = 20;

            GridFlea g = new GridFlea(size: SIZE, reward: INIT_REWARD);

            int expectedChange = 0;
            int expectedValue = (int)(g.GetReward() * g.GetSize() * expectedChange);
            Assert.AreEqual(expectedValue, g.Value(), "GridFlea value not correct before move");
        }

        [TestMethod]
        public void Value_AfterMove()
        {
            const int INIT_REWARD = 15;
            const uint SIZE = 20;
            const int INIT_X = 50;
            const int INIT_Y = -25;

            GridFlea g = new GridFlea(x: INIT_X, y: INIT_Y, size: SIZE, reward: INIT_REWARD);
            const int MOVE_AMOUNT = 3;
            g.Move(MOVE_AMOUNT);

            int expectedChange = Math.Abs(INIT_X - g.GetX()) + Math.Abs(INIT_Y - g.GetY());
            int expectedValue = (int)(g.GetReward() * g.GetSize() * expectedChange);
            Assert.AreEqual(expectedValue, g.Value(), "GridFlea value not correct after move");
        }

        [TestMethod]
        public void Value_AfterMultipleMoves_IsNegative()
        {
            const int INIT_REWARD = 15;
            const uint SIZE = 20;
            const int INIT_X = 50;
            const int INIT_Y = -25;

            GridFlea g = new GridFlea(x: INIT_X, y: INIT_Y, size: SIZE, reward: INIT_REWARD);
            const int MOVE_AMOUNT1 = 3;
            g.Move(MOVE_AMOUNT1);

            const int MOVE_AMOUNT2 = 20;
            g.Move(MOVE_AMOUNT2);

            int expectedChange = Math.Abs(INIT_X - g.GetX()) + Math.Abs(INIT_Y - g.GetY());
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

            GridFlea.Axis direction = g.GetDirection();
            int moveAmount = direction == GridFlea.Axis.X ? GridFlea.BOUND_X : GridFlea.BOUND_Y;
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
