// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

#ifndef INFEST_H
#define INFEST_H

using namespace std;

#include "GridFlea.h"

class Infest {
public:
    Infest(unsigned int severity);

    ~Infest();

    Infest(const Infest &src);

    Infest(Infest &&src);

    Infest &operator=(const Infest &src);

    Infest &operator=(Infest &&src);

    void move(int p);

    int minValue();

    int maxValue();

private:
    const unsigned int REVIVE_ENERGY = 10;
    unsigned int severity = 0;

    GridFlea **fleas = nullptr;

    enum Extreme {
        MIN,
        MAX
    };

    int extremeValue(Extreme e);

    void reproduce();

    void validatePopulation();

    GridFlea *birthGridFlea(int index) const;

    GridFlea *getGridFlea(int index) const;

    void copySemantic(const Infest &src);

    void swap(Infest &src, bool zeroOut);

    void deleteSemantic();
};

#endif