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

    this->fleas = new GridFlea[this->severity];
    for (int i = 0; i < severity; i++) {
        fleas[i] = birthGridFlea(i);
    }
}

Infest::~Infest() {
    delete[] fleas;
}

void Infest::move(int p) {
    bool moved = false;

    for (int i = 0; i < severity; i++) {
        GridFlea flea = fleas[i];

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
        GridFlea flea = fleas[i];

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
        GridFlea flea = fleas[i];

        if (first) {
            currMax = flea.value();
            first = false;
        } else {
            currMax = min(currMax, flea.value());
        }
    }

    return currMax;
}

GridFlea Infest::birthGridFlea(int nonce) {
    int val = nonce * (int) severity;

    int x = val % (2 * nonce) * (int) pow(-1, nonce);
    int y = val % (3 * nonce) * (int) pow(-1, nonce);
    unsigned int size = val % (4 * nonce) * x;
    int reward = val % (5 * nonce) * y;
    int energy = val % (6 * nonce) + x + y;

    return GridFlea(x, y, size, reward, energy);
}
