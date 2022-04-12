// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

#ifndef INFEST_H
#define INFEST_H

using namespace std;

#include "GridFlea.h"

class Infest
{
public:
	Infest(unsigned int severity);
	~Infest();

    // Move semantic
    // Copy constructor

	void move(int p);

	int minValue();
	int maxValue();

private:
	unsigned int severity;

	GridFlea *fleas;
	unsigned int generations = 0;

	void reproduce();

    GridFlea birthGridFlea(int index);
};

#endif