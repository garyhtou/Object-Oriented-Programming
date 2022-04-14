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

    fleas = new GridFlea *[severity];
    for (int i = 0; i < this->severity; i++) {
        fleas[i] = birthGridFlea(i);
    }
}

Infest::Infest(const Infest &src) {
    copySemantic(src);
}

Infest::Infest(Infest &&src) {
    swap(src, true);
}

Infest &Infest::operator=(const Infest &src) {
    if (this == &src) return *this;

    deleteSemantic();
    copySemantic(src);
    return *this;
}

Infest &Infest::operator=(Infest &&src) {
    if (this == &src) return *this;

    swap(src, false);
    return *this;
}

Infest::~Infest() {
    deleteSemantic();
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
        if (!flea.isActive()) continue;

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
    const int THRESHOLD = (int) round(severity / 2);
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
    int x = nonce * 2;
    int y = nonce * 3;
    unsigned int size = nonce * 4;
    int reward = nonce * 6;
    int energy = nonce * 4;

    return new GridFlea(x, y, size, reward, energy);
}

GridFlea Infest::getGridFlea(int index) const {
    return *fleas[index];
}

void Infest::copySemantic(const Infest &src) {
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

void Infest::deleteSemantic() {
    for (int i = 0; i < severity; i++) {
        delete fleas[i];
    }
    delete[] fleas;
}


// ownership transfer