// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

#ifndef INFEST_H
#define INFEST_H

using namespace std;

#include "GridFlea.h"

class Infest {
public:
    //
    Infest(unsigned int severity);

    ~Infest();

    Infest(const Infest &src);
    Infest(Infest &&src);

    void move(int p);

    int minValue();

    int maxValue();

private:
    const unsigned int REVIVE_ENERGY = 10;
    unsigned int severity;

    GridFlea **fleas = nullptr;

    enum Extreme {
        MIN,
        MAX
    };

    int extremeValue(Extreme e);

    void reproduce();

    void validatePopulation();

    GridFlea *birthGridFlea(int index) const;

    GridFlea getGridFlea(int index) const;
};

#endif