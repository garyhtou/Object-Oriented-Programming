#include <iostream>
#include "inFest.h"
#include <vector>
#include <memory>

using namespace std;

const int NUM_SHARED_INFESTS = 5;
const string PRINT_PREFIX = "|  ";
const string CLONED_PREFIX = PRINT_PREFIX + "  Cloned Infest:         ";
const string ORIGINAL_PREFIX = PRINT_PREFIX + "  Original Infest:       ";
const int MOVE_LIMIT = 20;
const int SEVERITY_LIMIT = 100;

unique_ptr<Infest> printInitStats(unique_ptr<Infest> infest) {
    cout << "  It was created with a min value of " << infest->minValue()
         << " and a max value of " << infest->maxValue() << "." << endl;
    return infest;
}

unique_ptr<Infest> createInfest(int id) {
    cout << "Creating Infest #" << id
         << ". Transferring sole ownership to caller." << endl;
    unsigned int severity = rand() % SEVERITY_LIMIT + 1;
    return make_unique<Infest>(severity);
}


vector<shared_ptr<Infest>> createInfestCollection(int num) {
    vector<shared_ptr<Infest>> infests = vector<shared_ptr<Infest>>();
    for (int i = 0; i < num; i++) {
        unique_ptr<Infest> infest = createInfest(i + 1);
        infest = printInitStats(move(infest));

        // Convert to shared pointer
        shared_ptr<Infest> sharedInfest = move(infest);
        infests.push_back(sharedInfest);
    }
    return infests;
}

void move(shared_ptr<Infest> infest, unsigned int times, string type = "") {
    for (int i = 0; i < times; i++) {
        int amount = rand() % MOVE_LIMIT - (MOVE_LIMIT / 2);
        infest->move(amount);
    }
    cout << PRINT_PREFIX << "Moved ";
    if (type.length() > 0) {
        cout << type << " Infest ";
    }
    cout << times << " times" << endl;
}

void moveBoth(shared_ptr<Infest> &infest1, shared_ptr<Infest> &infest2,
              unsigned int times) {
    for (int i = 0; i < times; i++) {
        int amount = rand() % (int) (MOVE_LIMIT / 2) - (int) (MOVE_LIMIT / 4);
        infest1->move(amount);
        infest2->move(amount);
    }
    cout << PRINT_PREFIX << "Moved both Infests the same amount " << times
         << " times" << endl;
}

void printVals(shared_ptr<Infest> infest, string prefix) {
    cout << prefix << "Value. Min: " << infest->minValue() << "\t Max: "
         << infest->maxValue() << endl;
}

const int MOVE1_TIMES = 5;
const int MOVE2_TIMES = 3;
const int MOVE3_TIMES = 5;
const int MOVE4_TIMES = 2;

void run(shared_ptr<Infest> &infest, int id) {
    cout << "\n======= Running Infest #" << id << " =======" << endl;

    move(infest, MOVE1_TIMES);
    printVals(infest, PRINT_PREFIX);

    move(infest, MOVE2_TIMES);
    printVals(infest, PRINT_PREFIX);

    shared_ptr<Infest> clonedInfest = infest;
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

    cout << "=== Finished Running Infest #" << id << " ===\n" << endl;
}

int main(int argc, char *argv[]) {
    vector<shared_ptr<Infest>> infests = createInfestCollection(
            NUM_SHARED_INFESTS);

    for (int i = 0; i < infests.size(); i++) {
        run(infests.at(i), i + 1);
    }

    // Remove all Infests from the vector
    int count = 0;
    while (!infests.empty()) {
        cout << "Removing Infest #" << count + 1 << " from vector" << endl;
        infests.pop_back();
        count++;
    }
}
