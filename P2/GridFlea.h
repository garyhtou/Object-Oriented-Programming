// Gary Tou
// April 6th, 2022
// CPSC 3200, P2

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

    GridFlea(int x = 0, int y = 0, unsigned int size = 10, int reward = 10, int energy = 1);

    void reset();

    void revive(unsigned int energy);

    void move(int p);

    int value();

    State getState();

    bool isActive();

    bool isInactive();

    bool isDead();

private:
    static const int BOUND_X = 1000;
    static const int BOUND_Y = 1000;
    static const int UNENERGETIC_MOVE_AMT = 1;

    unsigned int size;

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

    bool isOutOfBounds() const;

    bool isEnergetic() const;

    void switchDirection();
};

#endif