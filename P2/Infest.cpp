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
    bool moved = false;

    for (int i = 0; i < severity; i++) {
        GridFlea flea = getGridFlea(i);

        if (!flea.isDead()) {
            flea.move(p);
            moved = true;
        }
    }

    if (!moved) {
        // TODO: reproduce or error?
    }
}

int Infest::minValue() {
    int currMin = 0;
    bool first = true;

    for (int i = 0; i < severity; i++) {
        GridFlea flea = getGridFlea(i);

        if (first) {
            currMin = flea.value();
            first = false;
        } else {
            currMin = min(currMin, flea.value());
        }
    }

    return currMin;
}

int Infest::maxValue() {
    int currMax = 0;
    bool first = true;

    for (int i = 0; i < severity; i++) {
        GridFlea flea = getGridFlea(i);

        if (first) {
            currMax = flea.value();
            first = false;
        } else {
            currMax = min(currMax, flea.value());
        }
    }

    return currMax;
}


GridFlea *Infest::birthGridFlea(int nonce) const {
    int val = nonce * ((int) severity + 5) % 10;
    int negative = (int) pow(-1, nonce);

    int x = val % (2 * nonce) * negative;
    int y = val % (3 * nonce) * negative;
    unsigned int size = val % (4 * nonce) * x;
    int reward = val % (5 * nonce) * y;
    int energy = val % (6 * nonce) + x + y;

    return new GridFlea(x, y, size, reward, energy);
}

GridFlea Infest::getGridFlea(int index) const {
    return *fleas[index];
}
