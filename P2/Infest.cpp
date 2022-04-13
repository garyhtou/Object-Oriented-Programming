// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

#include <stdexcept>
#include <cmath>
#include <algorithm>
#include "Infest.h"
#include "GridFlea.h"

Infest::Infest(unsigned int severity) {
    if (severity <= 0) {
        throw invalid_argument("Severity can not be less than or equal to one");
    }
    this->severity = severity;

    for (int i = 0; i < this->severity; i++) {
        fleas[i] = birthGridFlea(i);
    }
}

Infest::~Infest() {
    for (int i = 0; i < severity; i++) {
        delete fleas[i];
    }
    delete[] fleas;
}


void Infest::move(int p) {
    validatePopulation();

    for (int i = 0; i < severity; i++) {
        GridFlea flea = getGridFlea(i);

        if (flea.isActive()) {
            flea.move(p);
        }
    }
}

int Infest::minValue() {
    validatePopulation();
    return extremeValue(MIN);
}

int Infest::maxValue() {
    validatePopulation();
    return extremeValue(MAX);
}

int Infest::extremeValue(Extreme e) {
    int currExtreme = 0;
    bool first = true;

    for (int i = 0; i < severity; i++) {
        GridFlea flea = getGridFlea(i);

        if (first) {
            currExtreme = flea.value();
            first = false;
        } else if (MIN == e) {
            currExtreme = std::min(currExtreme, flea.value());
        } else if (MAX == e) {
            currExtreme = std::max(currExtreme, flea.value());
        }
    }

    return currExtreme;
}

void Infest::reproduce() {
    for (int i = 0; i < severity; i++) {
        GridFlea flea = getGridFlea(i);

        if (flea.isInactive()) {
            flea.revive(REVIVE_ENERGY);
        } else if (flea.isDead()) {
            // Replace the GridFlea with a new one
            delete fleas[i];
            fleas[i] = birthGridFlea(i);
        }
    }
}

void Infest::validatePopulation() {
    const int THRESHOLD = round(severity / 2);
    unsigned int deadCount = 0;

    for (int i = 0; i < severity; i++) {
        GridFlea flea = getGridFlea(i);

        if (flea.isDead()) {
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


GridFlea *Infest::birthGridFlea(int nonce) const {
    // Arbitrary formulas for creating GridFleas with a wide distribution of initial values.
    // https://www.desmos.com/calculator/fnzcs8g0jt
    nonce += 2;
    int val = (((int) severity + 11) % (nonce * 97)) * (nonce % 7) + nonce;
    int negative = (int) (pow(-1, nonce) * pow(-1, val));

    int x = ((val * 3) % (int) (nonce * negative * cos(nonce)));
    int y = ((val * 3) % (int) (nonce * negative * sin(nonce)));
    unsigned int size = (val * 7 + severity) % x;
    int reward = (int) (((val * 37 + x) % (int) (pow(nonce, 1.5) / severity + nonce)) * sin(x));
    int energy = (int) (((val * 3 + x) % (int) sqrt(nonce * 97)) * pow(negative, 1.99 * nonce));

    return new GridFlea(x, y, size, reward, energy);
}

GridFlea Infest::getGridFlea(int index) const {
    return *fleas[index];
}
