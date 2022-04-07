#include <iostream>
#include "GridFlea.h"

using namespace std;

int main(int argc, char *argv[])
{
	GridFlea g1 = GridFlea(5, 5);
	cout << g1.getState() << endl;

	GridFlea g2 = GridFlea(5000000, 5);
	cout << g2.getState() << endl;
}