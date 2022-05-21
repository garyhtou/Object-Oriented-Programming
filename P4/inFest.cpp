// Gary Tou
// April 6th, 2022
// CPSC 3200, P4

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// This Infest class represents an infestation (collection) of GridFleas. It
// allows the entire infestation to `move()` in unison and for the min and max
// values of the infestation to be calculated. Infestations will never die out.
//
// Infest supports numerous overloaded operators including comparison (>, <,
// >, <=, ==, !=),  and addition with GridFleas and Infests.
// All addition and subtraction operators support both destructive and
// non-destructive methods.
//
// Error Handling for GridFlea is done through throwing Exceptions.

#include <stdexcept>
#include <cmath>
#include <algorithm>
#include "inFest.h"
#include "gridFlea.h"

/*
 * Preconditions: `severity` must be greater than zero
 * Postconditions: none
 */
Infest::Infest(unsigned int severity) {
    if (severity <= 0) {
        throw invalid_argument("Severity can not be less than or equal to one");
    }
    this->severity = severity;

    fleas = new GridFlea *[severity];
    for (int i = 0; i < this->severity; i++) {
        fleas[i] = birthGridFlea(i);
    }
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest::Infest(const Infest &src) {
    copy(src);
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest::Infest(Infest &&src) {
    moveAndZeroOut(src);
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest &Infest::operator=(const Infest &src) {
    if (this == &src) return *this;

    destroy();
    copy(src);
    return *this;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest &Infest::operator=(Infest &&src) {
    if (this == &src) return *this;

    swap(src);
    return *this;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest::~Infest() {
    destroy();
}

/*
 * Preconditions: none
 * Postconditions: none
 */
void Infest::move(int p) {
    validatePopulation();

    for (int i = 0; i < severity; i++) {
        GridFlea *flea = getGridFlea(i);

        if (flea->isActive()) {
            flea->move(p);
        }
    }
}

/*
 * Preconditions: none
 * Postconditions: none
 */
int Infest::minValue() {
    validatePopulation();
    return extremeValue(MIN);
}

/*
 * Preconditions: none
 * Postconditions: none
 */
int Infest::maxValue() {
    validatePopulation();
    return extremeValue(MAX);
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest Infest::operator+(const Infest &rhs) const {
    Infest temp(*this);

    for (int i = 0; i < rhs.severity; i++) {
        temp.appendGridFlea(*rhs.getGridFlea(i));
    }

    return temp;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest &Infest::operator+=(const Infest &rhs) {
    for (int i = 0; i < rhs.severity; i++) {
        appendGridFlea(*rhs.getGridFlea(i));
    }

    return *this;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest Infest::operator+(const GridFlea &rhs) const {
    Infest temp(*this);
    temp.appendGridFlea(rhs);
    return temp;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest &Infest::operator+=(const GridFlea &rhs) {
    appendGridFlea(rhs);
    return *this;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest &Infest::operator++() { // prefix
    GridFlea *flea = birthGridFlea(severity);
    appendGridFlea(*flea);
    delete flea;

    return *this;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
Infest Infest::operator++(int _x) { // postfix
    Infest temp(*this);
    ++(*this);
    return temp;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool Infest::operator==(const Infest &rhs) const {
    return this->extremeRange() == rhs.extremeRange();
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool Infest::operator!=(const Infest &rhs) const {
    return this->extremeRange() != rhs.extremeRange();
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool Infest::operator<(const Infest &rhs) const {
    return this->extremeRange() < rhs.extremeRange();
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool Infest::operator>(const Infest &rhs) const {
    return this->extremeRange() > rhs.extremeRange();
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool Infest::operator<=(const Infest &rhs) const {
    return this->extremeRange() <= rhs.extremeRange();
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool Infest::operator>=(const Infest &rhs) const {
    return this->extremeRange() >= rhs.extremeRange();
}

int Infest::extremeValue(Extreme e) const {
    int currExtreme = 0;
    bool first = true;

    for (int i = 0; i < severity; i++) {
        GridFlea *flea = getGridFlea(i);
        if (!flea->isActive()) continue;

        if (first) {
            currExtreme = flea->value();
            first = false;
        } else if (MIN == e) {
            currExtreme = min(currExtreme, flea->value());
        } else if (MAX == e) {
            currExtreme = max(currExtreme, flea->value());
        }
    }

    return currExtreme;
}

int Infest::extremeRange() const {
    return extremeValue(MAX) - extremeValue(MIN);
}

void Infest::reproduce() {
    for (int i = 0; i < severity; i++) {
        GridFlea *flea = getGridFlea(i);

        if (flea->isInactive()) {
            flea->revive(REVIVE_ENERGY);
        } else if (flea->isDead()) {
            // Replace the GridFlea with a new one
            delete fleas[i];
            fleas[i] = birthGridFlea(i);
        }
    }
}

void Infest::validatePopulation() {
    const int THRESHOLD = (int) round(severity / 2);
    unsigned int deadCount = 0;

    for (int i = 0; i < severity; i++) {
        GridFlea *flea = getGridFlea(i);

        if (flea->isDead()) {
            deadCount++;
        }

        // Optimized for performance
        if (deadCount > THRESHOLD) {
            break;
        }
    }

    if (deadCount > THRESHOLD) {
        reproduce();
    }
}


const int X_MULTIPLIER = 2;
const int Y_MULTIPLIER = 3;
const int ACTIVE_BOUND_MULTIPLIER = 150;
const int SIZE_MULTIPLIER = 2;
const int REWARD_MULTIPLIER = 15;
const int ENERGY_MULTIPLIER = 2;
const int NONCE_LIMIT = 25;

GridFlea *Infest::birthGridFlea(int nonce) const {
    nonce = (nonce % NONCE_LIMIT) + 1;
    int x = nonce * X_MULTIPLIER;
    int y = nonce * Y_MULTIPLIER;
    unsigned int activeBound = nonce * ACTIVE_BOUND_MULTIPLIER;
    unsigned int size = nonce * SIZE_MULTIPLIER;
    int reward = nonce * REWARD_MULTIPLIER;
    int energy = nonce * ENERGY_MULTIPLIER;

    return new GridFlea(x, y, activeBound, size, reward, energy);
}

GridFlea *Infest::getGridFlea(int index) const {
    return fleas[index];
}

void Infest::appendGridFlea(const GridFlea &src) {
    GridFlea **temp = new GridFlea *[severity + 1];
    for (int i = 0; i < severity; i++) {
        temp[i] = fleas[i];
    }
    temp[severity] = new GridFlea(src);

    delete[] fleas;
    fleas = temp;
    severity++;
}

void Infest::copy(const Infest &src) {
    severity = src.severity;
    fleas = new GridFlea *[severity];

    for (int i = 0; i < severity; i++) {
        fleas[i] = new GridFlea(*src.fleas[i]);
    }
}

void Infest::swap(Infest &src, bool zeroOut) {
    unsigned int tempSeverity = src.severity;
    GridFlea **tempFleas = src.fleas;

    if (zeroOut) {
        src.severity = 0;
        src.fleas = nullptr;
    } else {
        src.severity = severity;
        src.fleas = fleas;
    }

    severity = tempSeverity;
    fleas = tempFleas;
}

void Infest::moveAndZeroOut(Infest &src) {
    swap(src, true);
}

void Infest::destroy() {
    for (int i = 0; i < severity; i++) {
        delete fleas[i];
    }
    delete[] fleas;
}

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// This Infest class does not have any explicit state variable, however, it
// encapsulates an array of pointers to GridFleas pointers. The cardinality of
// this array is determined by the `severity` of the Infest. This `severity`
// variable is set through the constructor.
// To aid in the creation of new GridFlea (with a wide distribution of initial
// values), the Infest class features a `birthGridFlea` method. More about this
// below.
//
// The Infest class supports C++'s standard copy and move semantics. Since this
// class contains heap data, the copy constructor, copy assignment operator,
// move constructor, and move assignment operator are all explicitly defined.
// They use helper methods such as `copy`, `swap`, `moveAndZeroOut`, and
// `destroy` to perform the copy and move operations as well as encourage
// functional decomposition and code reuse. For more information regarding copy
// and move semantics, please refer to the C++ documentation. This class's
// implementation of copy and move semantics are inline with the C++ standards.
// Side note: The `destroy()` method is also called by the destructor.
//
// >>>> `Infest()` (Constructor) <<<<
// The constructor of the Infest class takes in a single parameter, `severity`.
// This `severity` parameter is used to determine the cardinality of the array
// of GridFleas. The cardinality will never change in the lifetime of the
// GridFlea (it is rigid).
//
// >>>> `move()` <<<<<
// Before moving any GridFleas, a validation of the population is performed to
// ensure that at least half of the GridFleas are active. See the
// `validatePopulation()` section below for more information.
// The `move()` method takes in a single parameter, `p`, which is passed in as
// an argument to the `move()` method of every GridFlea in the Infest that is
// Active. Non-Active GridFleas are ignored.
//
// >>>> `minValue()` <<<<
// Before calculating the minimum value of the Infest, a validation of the
// population is performed to ensure that at least half of the GridFleas are
// active. See the `validatePopulation()` section below for more information.
// The `minValue()` method uses the `extremeValue()` method to determine the
// minimum value of all the Active GridFleas in the Infest.
//
// >>>> `maxValue()` <<<<
// Before calculating the maximum value of the Infest, a validation of the
// population is performed to ensure that at least half of the GridFleas are
// active. See the `validatePopulation()` section below for more information.
// The `maxValue()` method uses the `extremeValue()` method to determine the
// maximum value of all the Active GridFleas in the Infest.
//
// >>>> `extremeValue()` <<<<
// The `extremeValue()` method takes in a single parameter, `e`, which must be
// a value from the `Extreme` enum (MIN or MAX). `e` is used to determine
// whether the minimum or maximum value of the Infest is to be calculated.
// The `extremeValue()` method loops through all Active GridFleas and retrieves
// its `value()`. Then, keeps track of the min/max value of all the GridFleas.
// That min/max value is then returned.
//
// >>>> `validatePopulation()` <<<<
// The `validatePopulation()` method loops through all GridFleas in the Infest
// and determines whether more than half of the GridFleas are in a non-Active
// state (Inactive or Dead). If this is the case, the method will call
// `reproduce()`. Please see the `reproduce()` section below for more details.
//
// >>>> `reproduce()` <<<<
// The `reproduce()` method loops through all GridFleas in the Infest and
// will `revive()` any Inactive GridFleas, and replace any Dead GridFleas.
// Replacement of GridFleas is done by first deleting the existing GridFlea
// (since they are located on the heap), and then creating a new one using the
// `birthGridFlea()` method.
//
// >>>> `birthGridFlea()` <<<<
// The `birthGridFlea()` method takes in a single parameter, `nonce`, which is
// is used to deterministically generate a new GridFlea with a wide distribution
// of initial value. These initial values are calculated by simply multiplying
// the `nonce` by a constant. This method will return the pointer to a GridFlea
// on the heap.
//
// >>>> OVERLOADED OPERATORS <<<<<
// Infest supports numerous overloaded operators including comparison (>, <,
// >, <=, ==, !=),  and addition with GridFleas and Infests.
//
// Infest supports the addition of two Infest object. This will simply add the
// GridFleas from both Infest objects together. You can also add a GridFlea to
// an Infest object (myInfest + myGridFlea) to append that GridFlea to the
// existing array to GridFleas. The Infest class does not support subtraction
// that is extremely ambiguous and does not give the client enough predictable
// control over behavior of the Infest. Since GridFleas are managed internally
// by the infest and are all created by the infest on instantiation, it also
// support the pre/post-fix increment operator to easily increase the number of
// GridFleas in the Infest. A newly generated GridFlea will be appended to the
// array.
// All addition and subtraction operators support both destructive and
// non-destructive methods.