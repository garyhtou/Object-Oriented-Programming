// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

// Revision History:
// - April 6th, 2022: Port GridFlea from C# to C++
// - April 12th, 2022: Add an isEnergetic query method to separate some state logic from Active/Inactive/Dead

#include <stdexcept>
#include <cmath>
#include "GridFlea.h"

GridFlea::GridFlea(int x, int y, unsigned int size, int reward, int energy)
{
	initX = x;
	initY = y;
	initEnergy = energy;
	initReward = reward;

	this->size = size;

	setup();
}

void GridFlea::reset()
{
	if (isDead())
	{
		throw invalid_argument("Can not reset a Dead (deactivated) GridFlea");
	}

	setup();
}

void GridFlea::revive(unsigned int energy)
{
	if (!isInactive())
	{
		throw invalid_argument("Can not revive an Active or Dead (deactivated) GridFlea");
	}

	this->energy = energy;
	state = Active;
}

void GridFlea::move(int p)
{
	if (isDead())
	{
		throw invalid_argument("Can not move a Dead (deactivated) GridFlea");
	}

	int amount = (isEnergetic() || p == 0) ? p : UNENERGETIC_MOVE_AMT;

	if (direction == X)
	{
		if (abs(x) > BOUND_X - abs(amount))
		{
			state = Dead;
		}
		x += amount;
	}
	else
	{
		if (abs(y) > BOUND_Y - abs(amount))
		{
			state = Dead;
		}
		y += amount;
	}

	switchDirection();

	reward -= abs(amount);
	energy--;
}

int GridFlea::value()
{
	if (!isActive())
	{
		throw invalid_argument("Can not get value of an Inactive or Dead (deactivated) GridFlea");
	}

	return reward * size * getChange();
}

GridFlea::State GridFlea::getState()
{
	if (state == Active && !isEnergetic())
	{
		state = Inactive;
	}
	else if (state == Inactive && isEnergetic())
	{
		state = Active;
	} else if (state != Dead && isOutOfBounds())
	{
		state = Dead;
	}

	return state;
}

bool GridFlea::isActive()
{
	return getState() == Active;
}
bool GridFlea::isInactive()
{
	return getState() == Inactive;
}
bool GridFlea::isDead()
{
	return getState() == Dead;
}

void GridFlea::setup()
{
	x = initX;
	y = initY;
	energy = initEnergy;
	reward = initReward;

	state = Active;
	direction = X;
}

int GridFlea::getChange()
{
	return abs(initX - x) + abs(initY - y);
}

bool GridFlea::isOutOfBounds()
{
	return abs(x) > BOUND_X || abs(y) > BOUND_Y;
}

bool GridFlea::isEnergetic() const
{
    return energy > 0;
}

void GridFlea::switchDirection()
{
	direction = (direction == X) ? Y : X;
}
