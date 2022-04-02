// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

// TODO: CLASS AND INTERFACE INVARIANTS

using System;

namespace GridFlea
{
    public class GridFlea
    {
        private const int BOUND_X = 100;
        private const int BOUND_Y = 100;

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

            setup();
        }

        public void reset()
        {
            if (isDead())
            {
                throw new InvalidOperationException("Can not reset a Dead (deactivated) GridFlea");
            }

            setup();
        }

        public void revive(uint energy)
        {
            if (!isInactive())
            {
                throw new InvalidOperationException("Can not revive an Active or Dead (deactivated) GridFlea");
            }

            this.energy = (int)energy;
            this.state = State.Active;
        }


        public void move(int p)
        {
            if (isDead())
            {
                throw new InvalidOperationException("Can not move a Dead (deactivated) GridFlea");
            }

            int amount = isActive() ? p : 1;

            if (direction == Axis.X)
            {
                x += p;
            }
            else
            {
                y += p;
            }
            switchDirection();

            reward -= Math.Abs(amount);
            energy--;
        }

        public int value()
        {
            if (!isActive())
            {
                throw new InvalidOperationException("Can not get value of an Inactive or Dead (deactivated) GridFlea");
            }

            return reward * (int)size * getChange();
        }

        // PRIVATE METHODS
        private void setup()
        {
            x = initX;
            y = initY;
            energy = initEnergy;
            reward = initReward;

            state = State.Active;
            direction = Axis.X;
        }

        private int getChange()
        {
            return Math.Abs(initX - x) + Math.Abs(initY - y);
        }

        private bool isOutOfBounds()
        {
            return Math.Abs(x) > BOUND_X || Math.Abs(y) > BOUND_Y;
        }

        private State getState()
        {
            // State transitions
            if (state == State.Active && energy <= 0)
            {
                state = State.Inactive;
            }
            else if (state != State.Dead && isOutOfBounds())
            {
                state = State.Dead;
            }

            return state;
        }

        private bool isActive()
        {
            return getState() == State.Active;
        }
        private bool isInactive()
        {
            return getState() == State.Inactive;
        }
        private bool isDead()
        {
            return getState() == State.Dead;
        }

        private void switchDirection()
        {
            direction = (direction == Axis.X) ? Axis.Y : Axis.X;
        }

    }
}

// TODO: Unit tests

