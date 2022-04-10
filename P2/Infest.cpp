#include <stdexcept>
#include <cmath>
#include <algorithm>
#include "Infest.h"

Infest::Infest(unsigned int severity)
{
	if (severity <= 0)
	{
		throw invalid_argument("Severity can not be less than or equal to one");
	}

	this->severity = severity;

	fleas = new GridFlea[this->severity];
}

Infest::~Infest()
{
	// TODO: destructor
}

void Infest::move(int p)
{
	bool moved = false;

	for (int i = 0; i < severity; i++)
	{
		GridFlea flea = fleas[i];

		if (!flea.isDead())
		{
			flea.move(p);
			moved = true;
		}
	}

	if (!moved)
	{
		// TODO: reproduce or error?
	}
}

int Infest::minValue()
{
	int currMin;
	bool first = true;

	for (int i = 0; i < severity; i++)
	{
		GridFlea flea = fleas[i];

		if (first)
		{
			currMin = flea.value();
		}
		else
		{
			currMin = min(currMin, flea.value());
		}
	}

	return currMin;
}

int Infest::reduce(int value, GridFlea flea)
{
	return 0;
}

