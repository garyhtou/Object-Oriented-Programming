// Gary Tou
// March 28th, 2022
// CPSC 3200, P1

using System;
using System.Linq;

namespace GridFleaNS
{
    class P1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the GridFlea Driver!\n");

            GridFlea[] fleas = CreateGridFleas(10);

            run(fleas);
        }

        private static int MOVE1_TIMES = 10;
        private static int MOVE2_TIMES = 3;
        private static int MOVE3_RAND_LO_TIMES = 1;
        private static int MOVE3_RAND_HI_TIMES = 3;
        static void run(GridFlea[] fleas)
        {
            for (int i = 0; i < fleas.Length; i++)
            {
                int id = i + 1;
                GridFlea flea = fleas[i];
                Console.WriteLine($"======= Running GridFlea #{id} =======");

                AttemptToMove(flea, MOVE1_TIMES); // Move 10 times

                AttemptToRevive(flea);

                AttemptToMove(flea, MOVE2_TIMES); // Move 3 times

                AttemptToGetValue(flea);

                AttemptToReset(flea);

                Random rand = new Random();
                int moveTimes = rand.Next(MOVE3_RAND_LO_TIMES, MOVE3_RAND_HI_TIMES);
                AttemptToMove(flea, moveTimes);

                print($"This flea is currently {flea.GetState()}");
                Console.WriteLine($"=== Finished Running GridFlea #{id} ===\n\n");
            }
        }

        private const int MOVE_RAND_LO_AMT = -100;
        private const int MOVE_RAND_HI_AMT = 300;
        static void AttemptToMove(GridFlea flea, int numTimes = 1)
        {
            int timesMoved = 0;
            foreach (int _ in Enumerable.Range(1, numTimes))
            {
                if (flea.IsDead())
                {
                    break;
                }

                Random rand = new Random();
                int amount = rand.Next(MOVE_RAND_LO_AMT, MOVE_RAND_HI_AMT);

                flea.Move(amount);
                timesMoved++;
            }
            if (timesMoved > 0)
            {
                print($"Moved {timesMoved} times.");
            }
            if (timesMoved < numTimes)
            {
                print("Skipping move, this flea is Dead.");
            }
        }

        private const int REVIVE_ENERGY = 2;
        static void AttemptToRevive(GridFlea flea)
        {
            if (flea.IsInactive())
            {
                print("Reviving this inactive flea.");
                flea.Revive(REVIVE_ENERGY);
            }
            else if (flea.IsDead())
            {
                print("Can't revive flea since it's Dead.");
            }
        }

        static void AttemptToGetValue(GridFlea flea)
        {
            if (flea.IsActive())
            {
                print($"Value: {flea.Value()}");
            }
            else
            {
                print($"Can't get value since flea is {flea.GetState()}.");
            }
        }

        static void AttemptToReset(GridFlea flea)
        {
            if (!flea.IsDead())
            {
                print("Resetting flea.");
                flea.Reset();
            }
            else
            {
                print("Can't reset flea since it's Dead.");
            }
        }

        private const int POS_RAND_LIMIT = 500;
        private const int SIZE_RAND_LIMIT = 20;
        private const int REWARD_RAND_LIMIT = 1000;
        private const int ENERGY_RAND_LIMIT = 25;
        static GridFlea[] CreateGridFleas(int num)
        {
            GridFlea[] fleas = new GridFlea[num];

            for (int i = 0; i < fleas.Length; i++)
            {
                Random rand = new Random();
                int x = rand.Next(-POS_RAND_LIMIT, POS_RAND_LIMIT);
                int y = rand.Next(-POS_RAND_LIMIT, POS_RAND_LIMIT);
                uint size = (uint)rand.Next(0, SIZE_RAND_LIMIT);
                int reward = rand.Next(0, REWARD_RAND_LIMIT);
                int energy = rand.Next(0, ENERGY_RAND_LIMIT);

                fleas[i] = new GridFlea(x, y, size, reward, energy);
            }

            return fleas;
        }

        private const string PRINT_PREFIX = "|  > ";
        private static void print(string message)
        {
            Console.WriteLine(PRINT_PREFIX + message);
        }

    }
}
