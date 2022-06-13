// Gary Tou
// 5/23/2022
//
// Please see other file for answer to questions

class leapWorm : leapFrog
{
	public leapWorm(int a, int b) : base(a, b)
	{
		// ...
	}

	public override bool move(uint k)
	{
		if (alive) return false;
		numMoves++;

		bool inMaze = true;
		uint numCells = 1;
		while (inMaze && numCells < k)
		{
			inMaze = base.move(numCells++);
		}

		return alive = inMaze;
	}

	public override void reset()
	{
		m.clear();
		alive = true;
		diagonal = 1;
		x = y = numMoves = 0;
	}

	// `changeDirection` method is inherited from `leapFrog` class.
}
