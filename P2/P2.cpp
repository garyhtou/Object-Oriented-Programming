#include <iostream>
#include "Infest.h"

using namespace std;

const int NUM_INFESTS = 10;
const string PRINT_PREFIX = "|  > ";
const string CLONED_PREFIX = PRINT_PREFIX + "  Cloned Infest:    ";
const string ORIGINAL_PREFIX = PRINT_PREFIX + "  Original Infest:  ";
const int MOVE_LIMIT = 20;
const int SEVERITY_LIMIT = 100;

Infest **createInfests(int num) {
    Infest **infests = new Infest *[num];
    for (int i = 0; i < num; i++) {
        unsigned int severity = rand() % SEVERITY_LIMIT + 1;
        infests[i] = new Infest(severity);
    }
    return infests;
}

void deleteInfests(Infest **infests) {
    for (int i = 0; i < NUM_INFESTS; i++) {
        delete infests[i];
    }
    delete[] infests;
}

void move(Infest &infest, unsigned int times, string type = "") {
    for (int i = 0; i < times; i++) {
        int amount = rand() % MOVE_LIMIT - (MOVE_LIMIT / 2);
        infest.move(amount);
    }
    cout << PRINT_PREFIX << "Moved ";
    if (type.length() > 0) {
        cout << type << " Infest ";
    }
    cout << times << " times" << endl;
}

void moveBoth(Infest &infest1, Infest &infest2, unsigned int times) {
    for (int i = 0; i < times; i++) {
        int amount = rand() % (int) (MOVE_LIMIT / 2) - (int) (MOVE_LIMIT / 4);
        infest1.move(amount);
        infest2.move(amount);
    }
    cout << PRINT_PREFIX << "Moved both Infests " << times << " times" << endl;
}

void printVals(Infest &infest, string prefix) {
    cout << prefix << "Value. Min: " << infest.minValue() << "\t Max: "
         << infest.maxValue() << endl;
}

const int MOVE1_TIMES = 5;
const int MOVE2_TIMES = 3;
const int MOVE3_TIMES = 5;
const int MOVE4_TIMES = 2;

void run(Infest &infest, int id) {
    cout << "======= Running Infest #" << id << " =======" << endl;

    move(infest, MOVE1_TIMES);
    printVals(infest, PRINT_PREFIX);

    move(infest, MOVE2_TIMES);
    printVals(infest, PRINT_PREFIX);

    Infest clonedInfest = infest;
    cout << PRINT_PREFIX << endl;
    cout << PRINT_PREFIX << "Created a copy of Infest #" << id
         << " (Cloned Infest)" << endl;
    printVals(infest, ORIGINAL_PREFIX);
    printVals(clonedInfest, CLONED_PREFIX);

    moveBoth(infest, clonedInfest, MOVE3_TIMES);
    printVals(infest, ORIGINAL_PREFIX);
    printVals(clonedInfest, CLONED_PREFIX);

    move(infest, MOVE4_TIMES, "Original");
    printVals(infest, CLONED_PREFIX);

    move(clonedInfest, MOVE4_TIMES, "Cloned");
    printVals(clonedInfest, ORIGINAL_PREFIX);

    cout << "=== Finished Running Infest #" << id << " ===\n\n" << endl;
}

int main(int argc, char *argv[]) {
    Infest **infests = createInfests(NUM_INFESTS);

    for (int i = 0; i < NUM_INFESTS; i++) {
        run(*infests[i], i + 1);
    }

    deleteInfests(infests);
}
