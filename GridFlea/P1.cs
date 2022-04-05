// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

using System;

namespace GridFleaNS
{
    class P1
    {
        const bool SUMMARY = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the GridFlea Driver");

            GridFlea[] fleas = createGridFleas(50);

            for (int i = 0; i < fleas.Length; i++)
            {
                run(fleas[i], i);
            }
        }

        static void run(GridFlea flea, int i)
        {
            if (SUMMARY)
            {
            }
            else
            {
                Console.WriteLine($"Running GridFlea #{i}");
            }

            int value = flea.Value();
            if (SUMMARY)
            {
            }
            else
            {
                Console.WriteLine($"\tValue: #{flea.Value()}");
            }

        }


        static GridFlea[] createGridFleas(int num)
        {
            GridFlea[] fleas = new GridFlea[num];

            for (int i = 0; i < fleas.Length; i++)
            {
                Random rand = new Random();
                int x = rand.Next(-500, 500);
                int y = rand.Next(-500, 500);
                uint size = (uint)rand.Next(0, 20);
                int reward = rand.Next(0, 20);
                int energy = rand.Next(0, 10);

                fleas[i] = new GridFlea(x, y, size, reward, energy);
            }

            return fleas;
        }
    }
}
