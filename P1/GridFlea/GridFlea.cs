﻿// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// This GridFlea class represents a Flea which can `move()` within a cartesian
// grid — either along the X or Y Axis. The direction (Axis) of travel
// alternates per `move()` and defaults to the X-axis on intialization.
//
// Throughout the lifetime of a GridFlea, it can be either in an Active,
// Inactive, or Dead (deactivated) State. Its State largely depend on the
// position of the GridFlea and its energy.
//
// Error handling for GridFlea is done through throwing Exceptions.

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// The constructor allows for the creation of Active, Inactive, and Dead
// GridFleas since there are scenarios where a client may want an Inactive
// or Dead GridFlea right off the bat. If a GridFlea was intialized with
// values which result in an Inactive/Dead state, resetting the GridFlea
// will bring it back to it's original Inactive/Dead state. Default values
// are provided for the constructor.
//
// The GridFlea class provides multiple methods for retriving the state of the
// GridFlea. You can either use the `GetState()` method which returns a value
// from the `State` enum (Active, Inactive, Dead). Or, the GridFlea class also
// provides boolean methods for each state as syntatic sugar.
//   - `IsActive()`: Whether the GridFlea is in an Active state.
//   - `IsInactive()`: Whether the GridFlea is in an Inactive state.
//   - `IsDead()`: Whether the GridFlea is in a Dead state.


using System;

namespace GridFleaNS
{
    public class GridFlea
    {
        private const int BOUND_X = 1_000;
        private const int BOUND_Y = 1_000;
        private const int UNENERGETIC_MOVE_AMT = 1;

        private readonly uint size;

        private readonly int initX;
        private readonly int initY;
        private int x;
        private int y;

        private readonly int initReward;
        private int reward;

        private readonly int initEnergy;
        private int energy;

        public enum State
        {
            Active,
            Inactive,
            Dead
        }
        State state;

        private enum Axis
        {
            X,
            Y
        }
        Axis direction;

        /// PreConditions: none
        /// PostConditions:
        ///   - An Active Gridflea
        ///   - An Inative Gridflea (if energy <= 0)
        ///   - A Dead GridFlea (if initial x/y is outside of bounds)
        public GridFlea(int x = 0, int y = 0, uint size = 10, int reward = 10, int energy = 10)
        {
            initX = x;
            initY = y;
            initEnergy = energy;
            initReward = reward;

            this.size = size;

            Setup();
        }

        /// PreConditions: Gridflea must NOT be Dead
        /// PostConditions: Same state as when the GridFlea was initialized. See
        ///   GridFlea constructor for more details (above).
        public void Reset()
        {
            if (IsDead())
            {
                throw new InvalidOperationException("Can not reset a Dead (deactivated) GridFlea");
            }

            Setup();
        }

        /// PreConditions: GridFlea is Inactive
        /// PostConditions: GridFlea is Active
        public void Revive(uint energy)
        {
            if (!IsInactive())
            {
                throw new InvalidOperationException("Can not revive an Active or Dead (deactivated) GridFlea");
            }

            this.energy = (int)energy;
            this.state = State.Active;
        }


	    /// PreConditions: GridFlea is NOT Dead
	    /// PostConditions:
        ///   - GridFlea is Active (if X/Y are both on or within the bounds, and
	    ///       energy > 0)
        ///   - GridFlea is Inactive (if X/Y are both on or within the bounds,
	    ///       and energy <= 0)
        ///   - GridFlea is Dead (if either X/Y are outside the bounds)
        public void Move(int p)
        {
            if (IsDead())
            {
                throw new InvalidOperationException("Can not move a Dead (deactivated) GridFlea");
            }

            int amount = (IsActive() || p == 0) ? p : UNENERGETIC_MOVE_AMT;

            if (direction == Axis.X)
            {
                if (Math.Abs(x) > BOUND_X - Math.Abs(amount))
                {
                    state = State.Dead;
                }
                x += amount;
            }
            else
            {
                if (Math.Abs(y) > BOUND_Y - Math.Abs(amount))
                {
                    state = State.Dead;
                }
                y += amount;
            }
            SwitchDirection();

            reward -= Math.Abs(amount);
            energy--;
        }

        /// PreConditions: GridFlea is Active
        /// PostConditions: none (GridFlea remains in the same state)
        public int Value()
        {
            if (!IsActive())
            {
                throw new InvalidOperationException("Can not get value of an Inactive or Dead (deactivated) GridFlea");
            }

            return reward * (int)size * GetChange();
        }

        /// PreConditions: none
        /// PostConditions: none
        public State GetState()
        {
            // State transitions
            if (state == State.Active && energy <= 0)
            {
                state = State.Inactive;
            }
            else if (state == State.Inactive && energy > 0)
            {
                state = State.Active;
            }
            else if (state != State.Dead && IsOutOfBounds())
            {
                state = State.Dead;
            }

            return state;
        }

        /// PreConditions: none
        /// PostConditions: none
        public bool IsActive()
        {
            return GetState() == State.Active;
        }

        /// PreConditions: none
        /// PostConditions: none
        public bool IsInactive()
        {
            return GetState() == State.Inactive;
        }

        /// PreConditions: none
        /// PostConditions: none
        public bool IsDead()
        {
            return GetState() == State.Dead;
        }

        // PRIVATE METHODS
        private void Setup()
        {
            x = initX;
            y = initY;
            energy = initEnergy;
            reward = initReward;

            state = State.Active;
            direction = Axis.X;
        }

        private int GetChange()
        {
            return Math.Abs(initX - x) + Math.Abs(initY - y);
        }

        private bool IsOutOfBounds()
        {
            return Math.Abs(x) > BOUND_X || Math.Abs(y) > BOUND_Y;
        }

        private void SwitchDirection()
        {
            direction = (direction == Axis.X) ? Axis.Y : Axis.X;
        }

    }
}

// =============================================================================
// ------------------------ IMPLEMENTATION INVARIANTS --------------------------
// =============================================================================
// The GridFlea's overall state is managed with the following private variables:
//   - `x` (and `initX`)
//   - `y` (and `initY`)
//   - `size`. This never changes.
//   - `energy` (and `initEnergy`)
//   - `reward` (and `initReward`)
//   - state. Represented by an enum: Active, Inactive, and Dead.
//   - direction. Represented by an enum: X or Y.
//
// All state transitions are delayed until necessary and handled by the
// `GetState` method — except for a single `Active` -> `Dead` state transition
// in the `move()` method. That single state transition is necessary to handle
// the edge case of an Integer Overflow when a GridFlea tries to move far
// beyond its bound. This architecture was choosen as it allows for most state
// transition rules to be contains in one location (the `GetState()` method).
//
// The possible states and directions of a GridFlea is defined by the `State`
// and `Axis` enums, respectively. These enums allow for easy addition of
// additional states and axes when necessary. The possible states also have
// syntatic sugar methods (`IsActive()`, `IsInactive()`, `IsDead()`) to help
// the client write cleaner code.
//
// Both the GridFlea constructor and `Revive()` methods depend on the `Setup()`
// method which promotes code resuse by handling the setting of current
// variables equal to the initial variables defined in the constructor.
//
// Error handling is done through throwing Exceptions.
