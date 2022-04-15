// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

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

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// `GridFlea()` (Constructor):
//   - Creates a new GridFlea using the given arguments.
//   - Supports creation using default arguments
//   - Allows for creation of GridFleas in an Active, Inactive, or Dead state
//     since there are scenarios where a client may want an Inactive or Dead
//     GridFlea right off the bat.
// `reset()`:
//   - Resets the GridFlea back to its original state (as defined by the
//     arguments provided to the constructor).
//   - You can not reset a Dead GridFlea.
//   - If a GridFlea was initialized in an Inactive state, resetting
//     that GridFlea will bring it back to its original Inactive state.
// `revive()`:
//   - Provides an Inactive GridFlea with a new energy value.
//   - You can not revive a Dead GridFlea.
// `move()`:
//   - Moves the GridFlea in the direction it is currently facing.
//   - You can only move an Active GridFlea.
//   - Moving a GridFlea may result in it becoming Inactive or Dead.
// `value()`:
//   - Returns the value of the GridFlea.
//   - You can only get the value of an Active GridFlea.
//
// The GridFlea class provides multiple query methods for retrieving the state
// of the GridFlea. You can either use the `getState()` method which returns a
// value from the `State` enum (Active, Inactive, Dead). Or, the GridFlea class
// also provides boolean methods for each state as syntatic sugar.
//   - `isActive()`: Whether the GridFlea is in an Active state.
//   - `isInactive()`: Whether the GridFlea is in an Inactive state.
//   - `isDead()`: Whether the GridFlea is in a Dead state.

#ifndef GRIDFLEA_H
#define GRIDFLEA_H

using namespace std;

class GridFlea {
public:
    enum State {
        Active,
        Inactive,
        Dead
    };

    /*
     * Preconditions: none
     * Postconditions:
     *   - An Active GridFlea
     *   - An Inactive GridFlea (if initial x/y is outside of active bound). Active
     *     Bound is provided as an argument to the constructor.
     *   - A Dead GridFlea (if initial x/y is outside of grid bounds)
     */
    GridFlea(int x = 0, int y = 0, unsigned int activeBound = 800,
             unsigned int size = 10, int reward = 10, int energy = 1);

    /*
     * Preconditions: GridFlea must NOT be Dead
     * Postconditions: Same state as when the GridFlea was initialized. See GridFlea
     *   constructor for more information (above).
     */
    void reset();

    /*
     * Preconditions: GridFlea is NOT Dead
     * Postconditions: none (GridFlea remains in the same state)
     */
    void revive(unsigned int energy);

    /*
     * Preconditions: GridFlea is Active
     * Postconditions:
     *   - GridFlea is Active (if x/y are both on or within the grid bound)
     *   - GridFlea is Inactive (if either x/y is between the active bound and grid
     *     bound)
     *   - GridFlea is Dead (if either x/y is outside the grid bound)
     */
    void move(int p);

    /*
     * Preconditions: GridFlea is Active
     * Postconditions: none (GridFlea remains in the same state)
     */
    int value();

    /*
     * Preconditions: none
     * Postconditions: none
     */
    State getState();

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool isActive();

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool isInactive();

    /*
     * Preconditions: none
     * Postconditions: none
     */
    bool isDead();

private:
    static const int BOUND_X = 1000;
    static const int BOUND_Y = 1000;
    static const int UNENERGETIC_MOVE_AMT = 1;

    unsigned int size;
    unsigned int activeBound;

    int initX;
    int initY;

    int x;
    int y;

    int initReward;
    int reward;

    int initEnergy;
    int energy;

    State state = Active;

    enum Axis {
        X,
        Y
    };
    Axis direction;

    void setup();

    int getChange() const;

    bool isOutOfActiveBounds() const;

    bool isOutOfGridBounds() const;

    bool isEnergetic() const;

    void switchDirection();
};

#endif