// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

// Revision History:
// - P2. April 6th, 2022: Port GridFlea from C# to C++
// - P2. April 12th, 2022: Add an `isEnergetic()` query method to separate some state
//     logic from Active/Inactive/Dead
// - P2. April 14th, 2022: Add an `activeBound` to completely separate energy and
//     state

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// This GridFlea class represents a flea which can `move()` within a cartesian
// grid — either along the X or Y axis. The direction (Axis) of travel
// alternates per `move()` and defaults to the X axis on initialization.
//
// Throughout the lifetime of a GridFlea, it can either in an Active, Inactive,
// or Dead (deactivated) state. Its State largely depends on the position of the
// GridFlea.
//
// Error Handling for GridFlea is done through throwing Exceptions.

#include <stdexcept>
#include <cmath>
#include "GridFlea.h"

/*
 * Preconditions: none
 * Postconditions:
 *   - An Active GridFlea
 *   - An Inactive GridFlea (if initial x/y is outside of active bound). Active
 *     Bound is provided as an argument to the constructor.
 *   - A Dead GridFlea (if initial x/y is outside of grid bounds)
 */
GridFlea::GridFlea(int x, int y, unsigned int activeBound, unsigned int size,
                   int reward, int energy) {
    initX = x;
    initY = y;
    initEnergy = energy;
    initReward = reward;

    this->size = size;
    this->activeBound = activeBound;

    setup();
}

/*
 * Preconditions: GridFlea must NOT be Dead
 * Postconditions: Same state as when the GridFlea was initialized. See GridFlea
 *   constructor for more information (above).
 */
void GridFlea::reset() {
    if (isDead()) {
        throw invalid_argument("Can not reset a Dead (deactivated) GridFlea");
    }

    setup();
}

/*
 * Preconditions: GridFlea is NOT Dead
 * Postconditions: none (GridFlea remains in the same state)
 */
void GridFlea::revive(unsigned int energy) {
    if (isDead()) {
        throw invalid_argument("Can not revive a Dead (deactivated) GridFlea");
    }

    this->energy = (int) energy;
}

/*
 * Preconditions: GridFlea is Active
 * Postconditions:
 *   - GridFlea is Active (if x/y are both on or within the grid bound)
 *   - GridFlea is Inactive (if either x/y is between the active bound and grid
 *     bound)
 *   - GridFlea is Dead (if either x/y is outside the grid bound)
 */
void GridFlea::move(int p) {
    if (!isActive()) {
        throw invalid_argument(
                "Can not move an Inactive or Dead (deactivated) GridFlea");
    }

    int amount = (isEnergetic() || p == 0) ? p : UNENERGETIC_MOVE_AMT;

    if (direction == X) {
        if (abs(x) > BOUND_X - abs(amount)) {
            state = Dead;
        }
        x += amount;
    } else {
        if (abs(y) > BOUND_Y - abs(amount)) {
            state = Dead;
        }
        y += amount;
    }

    switchDirection();

    reward -= abs(amount);
    energy--;
}

/*
 * Preconditions: GridFlea is Active
 * Postconditions: none (GridFlea remains in the same state)
 */
int GridFlea::value() {
    if (!isActive()) {
        throw invalid_argument(
                "Can not get value of an Inactive or Dead (deactivated) GridFlea");
    }

    return reward * size * getChange();
}

/*
 * Preconditions: none
 * Postconditions: none
 */
GridFlea::State GridFlea::getState() {
    if (state == Dead) return state;

    if (isOutOfGridBounds()) {
        state = Dead;
    } else if (isOutOfActiveBounds()) {
        state = Inactive;
    } else {
        state = Active;
    }

    return state;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool GridFlea::isActive() {
    return getState() == Active;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool GridFlea::isInactive() {
    return getState() == Inactive;
}

/*
 * Preconditions: none
 * Postconditions: none
 */
bool GridFlea::isDead() {
    return getState() == Dead;
}

/*
 * =============================================================================
 * ----------------------------- PRIVATE METHODS -------------------------------
 * =============================================================================
 */

void GridFlea::setup() {
    x = initX;
    y = initY;
    energy = initEnergy;
    reward = initReward;

    state = Active;
    direction = X;
}

int GridFlea::getChange() const {
    return abs(initX - x) + abs(initY - y);
}

bool GridFlea::isOutOfActiveBounds() const {
    return abs(x) > activeBound || abs(y) > activeBound;
}

bool GridFlea::isOutOfGridBounds() const {
    return abs(x) > BOUND_X || abs(y) > BOUND_Y;
}

bool GridFlea::isEnergetic() const {
    return energy > 0;
}

void GridFlea::switchDirection() {
    direction = (direction == X) ? Y : X;
}

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// The GridFlea's overall state is dependent on the following private variables:
//   - `x` (and `initX`)
//   - `y` (and `initY`)
//   - `size` — This never changes.
//   - `activeBound` — This never changes.
//   - `energy` (and `initEnergy`)
//   - `reward` (and `initReward`)
//   - `state` — Represented by an enum: Active, Inactive, Dead
//   - `direction` — Represented by an enum: X, Y
// All of these variables, expect for `state` and `direction`, are initialized
// through the constructor and have default values.
//
// >>>> STATE TRANSITIONS <<<<
// All state transitions are delayed until necessary and handled by the
// `getState()` method — except for a single state transition to Dead in the
// `move()` method. That state transition if necessary is necessary to handle
// the edge case of an Integer overflow when a GridFlea tries to move far beyond
// its bound. This architecture was chosen as it allows for most state
// transitions rules to be contained in one location (the `getState()` method).
//
// The possible states and directions of a GridFlea are defined by the `State`
// and `Axis` enums, respectively. These enums allow for easy additions of
// additional states and axes when necessary. The possible states also have
// syntactic sugar query methods (`isActive()`, `isInactive()`, `isDead()`) to
// help the client write cleaner code.
//
// >>>> ENERGY <<<<
// In addition to the main state (Active, Inactive, Dead), GridFleas also have
// a notion of "energy". A GridFlea can either be Energetic or Unenergetic.
// Whether a GridFlea is Energetic or Unenergetic is determined by its energy
// value — which decreases per `move()`. When energy <= 0, the GridFlea becomes
// Unenergetic and remains in that state until its `revive()` is called.
//
// >>>> REWARD <<<<<
// The reward of a GridFlea is an indicator for how far a GridFlea was moved.
// It begins at an initial value (`initReward`) and decreases by the number of
// squares moved.
//
// >>>> VALUE <<<<
// The value of a GridFlea is a calculation containing the reward, size, and
// change in position.
//
// >>>> BOUND CHECKING <<<<
// There are two bounds within a GridFlea: The grid bounds and the active
// bounds. The grid bounds are fixed for all GridFleas, however, the active
// bounds (which determines whether a GridFlea is Active or Inactive) can be set
// via the constructor. An active bound greater than the grid bound would
// indicate that the GridFlea can not come Inactive and will only transition
// directly to a Dead state.
// A GridFlea is considered to be out of bounds when the absolute value of the
// current position extends the relevant bound (either grid bound or active =
// bound).
//
// >>>> MOVE() <<<<<
// Only Active GridFleas can move. A GridFlea can move in one of two directions
// (X or Y). The GridFlea's facing direction (current direction) is determined
// by the `direction` (a value of the Direction enum). Upon initialization, the
// GridFlea is facing the X axis. However, it will alternate between X and Y
// after every `move()`.
// The amount of squares moved will be influenced by the argument `p`. An
// Energetic GridFlea will be allowed to move `p` squares when requested,
// however, an Unenergetic GridFlea will only be allowed to move 1 square (or 0,
// if `p` is equal 0). A movement of zero may represent a GridFlea skipping this
// move cycle — which is possible even if it is Unenergetic.
// Bounds checking is also performed to ensure that a GridFlea is marked as Dead
// when it moves beyond the grid bounds. See the Bounds Checking section for
// more details.
// After moving, the GridFlea's rewards and energy values are updated. See those
// sections for more details.
//
// >>>> REVIVE() <<<<<
// Dead GridFleas can NOT be revived. Upon reviving a GridFlea, its energy is
// set to the value provided by the parameter `energy`. This will bring the
// GridFlea back to an Energetic state.
//
// >>>> RESET() <<<<<
// Dead GridFleas can NOT be reset. Upon resetting a GridFlea, all member
// variables are set back to their initial values. For example, `x` is set to
// `initX`. This means that it will also bring the GridFlea back to its initial
// state (Active, Inactive, or Dead). If a GridFlea was initialized as Inactive
// or Dead, it will go back to that state.
// This `reset()` method utilizes the `setup()` method which is shared by the
// constructor and performs the setting of current values to initial values.
//
// >>>> `GridFlea()` (Constructor) <<<<<
// The constructor takes in initial values for the GridFlea and saves them to
// private member variables. Then, it calls the `setup()` method to set the
// current values to the initial values.

// Error handling is done through throwing Exceptions.
