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
// Infest supports numerous overloaded operators including comparison (>, <,
// >, <=, ==, !=),  and addition with GridFleas and Infests.
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
//
// Infest supports numerous overloaded operators including comparison (>, <,
// >, <=, ==, !=),  and addition with GridFleas and Infests.
// All addition and subtraction operators support both destructive and
// non-destructive methods.

#ifndef INFEST_H
#define INFEST_H

using namespace std;

#include "gridFlea.h"

class Infest {
public:
    /*
     * Preconditions: `severity` must be greater than zero
     * Postconditions: none
     */
    Infest(unsigned int severity);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest(const Infest &src);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest(Infest &&src);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest &operator=(const Infest &src);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest &operator=(Infest &&src);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    ~Infest();

    /*
     * Preconditions: none
     * Postconditions: none
     */
    void move(int p);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    int minValue();

    /*
     * Preconditions: none
     * Postconditions: none
     */
    int maxValue();

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest operator+(const Infest &rhs) const;

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest &operator+=(const Infest &rhs);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest operator+(const GridFlea &rhs) const;

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest &operator+=(const GridFlea &rhs);

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest &operator++(); // prefix

    /*
     * Preconditions: none
     * Postconditions: none
     */
    Infest operator++(int x); // postfix

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool operator==(const Infest &rhs) const;

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool operator!=(const Infest &rhs) const;

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool operator<(const Infest &rhs) const;

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool operator>(const Infest &rhs) const;

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool operator<=(const Infest &rhs) const;

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool operator>=(const Infest &rhs) const;

private:
    const unsigned int REVIVE_ENERGY = 10;
    unsigned int severity = 0;

    GridFlea **fleas = nullptr;

    enum Extreme {
        MIN,
        MAX
    };

    int extremeValue(Extreme e) const;

    int extremeRange() const;

    void reproduce();

    void validatePopulation();

    GridFlea *birthGridFlea(int index) const;

    GridFlea *getGridFlea(int index) const;

    void appendGridFlea(const GridFlea &src);

    void copy(const Infest &src);

    void swap(Infest &src, bool zeroOut = false);

    void moveAndZeroOut(Infest &src);

    void destroy();
};

#endif