using namespace std;

#include "GridFlea.h"

class Infest
{
public:
	Infest(unsigned int severity);
	~Infest();

	void move(int p);

	int minValue();
	int maxValue();

private:
	unsigned int severity;

	GridFlea *fleas;
	unsigned int generations = 0;

	void reproduce();

	int reduce(int value, GridFlea flea);
};