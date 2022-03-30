// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

using System;

namespace GridFlea
{
    class P1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the GridFlea Driver");

            GridFlea[] fleas = createGridFleas(50);

            foreach (GridFlea flea in fleas)
            {
                Console.WriteLine(flea.value());
	        }
        }

        static GridFlea[] createGridFleas(int num)
        {
            GridFlea[] fleas = new GridFlea[num];

            for(int i = 0; i < fleas.Length; i++)
            {
                Random rand = new Random();
                int x = rand.Next(1, 10);
                int y = rand.Next(1, 10);

                fleas[i] = new GridFlea(x, y);
	        }

            return fleas;
	    }
    }
}
