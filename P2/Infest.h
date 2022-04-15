// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// This Infest class represents an infestation (collection) of GridFleas. It
// allows the entire infestation to `move()` in unison and for the min and max
// values of the infestation to be calculated. Infestations will never die out.
//
// Error Handling for GridFlea is done through throwing Exceptions.

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// `Infest()` (constructor):
//   - Creates a new Infest using the given arguments.
//   - Supports creation using default arguments
//   - Allows the client to defined the `severity` of the infestation. This
//     represents the number of fleas within this infestation.
// `move()`:
//   - Moves all fleas in the infestation in unison. Some fleas may move more
//     than others due to their own individual states.
//   - The amount to move is determined by the method argument `p` and is
//     applied to every flea in the infestation.
// `minValue()`:
//   - Calculates the minimum value of the entire infestation.
// `maxValue()`:
//   - Calculates the maximum value of the entire infestation.

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

    void copy(const Infest &src);

    void swap(Infest &src, bool zeroOut = false);

    void moveAndZeroOut(Infest &src);

    void destroy();
};

#endif