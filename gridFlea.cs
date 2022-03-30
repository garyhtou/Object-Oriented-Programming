// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

// TODO: CLASS AND INTERFACE INVARIANTS

using System;

namespace GridFlea
{
    public class GridFlea
    {
        private int initX;
        private int initY;
        private int boundX;
        private int boundY;

        private int x;
        private int y;

        private int size;
        private int reward;
        private int energy;

        enum State
        {
            Active,
            Inactive,
            Dead
        }
        State state = State.Active;


        public GridFlea(int x, int y)
        {
            setup(x, y);
        }

        public void reset(int x, int y)
        {
            setup(x, y);
        }

        public void revive(int energy)
        {
            if (energy <= 0)
            {
                throw new ArgumentException("Energy must be greater than zero");
            }
            if (!isInactive())
            {
                throw new InvalidOperationException("Can not revive an Active or Dead (deactivated) GridFlea");
	        }

            this.energy = energy;
            this.state = State.Active;
            // TODO: reset reward?
        }


        public void move(int p)
        {
            int amount = isActive() ? p : 1;

            // Choose a random direction (axis)
            Random rand = new Random();
            if (rand.NextDouble() >= 0.5)
            {
                this.x += amount;
            }
            else
            {
                this.y += amount;
            }

            this.reward -= amount;
            this.energy--;
        }

        public int value()
        {
            if (!isActive())
            {
                throw new InvalidOperationException("Can not get value of an Inactive or Dead (deactivated) GridFlea");
            }

            return reward * size * getChange();
        }

        private int getChange()
        {
            return (initX - x) + (initY - y);
        }

        private bool isOutOfBounds()
        {
            return Math.Abs(this.x) > boundX || Math.Abs(this.y) > boundY;
        }

        private void setup(int x, int y)
        {
            this.initX = x;
            this.initY = y;


            Random rand = new Random();
            this.size = rand.Next(1, 10);
            this.energy = rand.Next(1, 10);

            this.boundX = rand.Next(10, 100);
            this.boundY = rand.Next(10, 100);

            // TODO: reward
        }

        private State getState()
        {
            if (state == State.Active && this.energy <= 0)
            {
                this.state = State.Inactive;
            }
            else if (state != State.Dead && isOutOfBounds())
            {
                this.state = State.Dead;
            }

            return this.state;
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

    }
}

// TODO: Unit tests

