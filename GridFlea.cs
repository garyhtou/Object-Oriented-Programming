// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

// TODO: CLASS AND INTERFACE INVARIANTS

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

        enum State
        {
            Active,
            Inactive,
            Dead
        }
        State state;

        enum Axis
        {
            X,
            Y
        }
        Axis direction;

        public GridFlea(int x = 0, int y = 0, uint size = 10, int reward = 10, int energy = 10)
        {
            initX = x;
            initY = y;
            initEnergy = energy;
            initReward = reward;

            this.size = size;

            Setup();
        }

        public void Reset()
        {
            if (IsDead())
            {
                throw new InvalidOperationException("Can not reset a Dead (deactivated) GridFlea");
            }

            Setup();
        }

        public void Revive(uint energy)
        {
            if (!IsInactive())
            {
                throw new InvalidOperationException("Can not revive an Active or Dead (deactivated) GridFlea");
            }

            this.energy = (int)energy;
            this.state = State.Active;
        }


        public void Move(int p)
        {
            if (IsDead())
            {
                throw new InvalidOperationException("Can not move a Dead (deactivated) GridFlea");
            }

            int amount = IsActive() ? p : UNENERGETIC_MOVE_AMT;

            if (direction == Axis.X)
            {
                if (x + p > BOUND_X)
                {
                    state = State.Dead;
                }
                x += p;
            }
            else
            {
                if (y + p > BOUND_X)
                {
                    state = State.Dead;
                }
                y += p;
            }
            SwitchDirection();

            reward -= Math.Abs(amount);
            energy--;
        }

        public int Value()
        {
            if (!IsActive())
            {
                throw new InvalidOperationException("Can not get value of an Inactive or Dead (deactivated) GridFlea");
            }

            return reward * (int)size * GetChange();
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

        private State GetState()
        {
            // State transitions
            if (state == State.Active && energy <= 0)
            {
                state = State.Inactive;
            }
            else if (state != State.Dead && IsOutOfBounds())
            {
                state = State.Dead;
            }

            return state;
        }

        private bool IsActive()
        {
            return GetState() == State.Active;
        }
        private bool IsInactive()
        {
            return GetState() == State.Inactive;
        }
        private bool IsDead()
        {
            return GetState() == State.Dead;
        }

        private void SwitchDirection()
        {
            direction = (direction == Axis.X) ? Axis.Y : Axis.X;
        }

    }
}

// TODO: Unit tests

