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

        static void run(GridFlea[] fleas)
        {
            for (int i = 0; i < fleas.Length; i++)
            {
                int id = i + 1;
                GridFlea flea = fleas[i];
                Console.WriteLine($"=== Running GridFlea #{id} ===");

                AttemptToMove(flea, 10); // Move 10 times

                AttemptToRevive(flea);

                AttemptToMove(flea, 3); // Move 3 times

                AttemptToGetValue(flea);

                AttemptToReset(flea);

                Random rand = new Random();
                int moveTimes = rand.Next(1, 3);
                AttemptToMove(flea, moveTimes);

                print($"This flea is currently {flea.GetState()}, located at ({flea.GetX()}, {flea.GetY()}), facing in the {flea.GetDirection()}-axis, has a size of {flea.GetSize()}, energy of {flea.GetEnergy()}, and reward of {flea.GetReward()}.");
                Console.WriteLine($"=== Finished Running GridFlea #{id} ===\n\n");
            }
        }

        static void AttemptToMove(GridFlea flea, int numTimes = 1)
        {
            foreach (int _ in Enumerable.Range(1, numTimes))
            {
                if (flea.IsDead())
                {
                    print("Skipping move, this flea is dead.");
                    break;
                }

                Random rand = new Random();
                int amount = rand.Next(-100, 300);

                flea.Move(amount);
                print($"Moved to ({flea.GetX()}, {flea.GetY()})");
            }
        }

        static void AttemptToRevive(GridFlea flea)
        {
            if (flea.IsInactive())
            {
                print("Reviving this inactive flea.");
                flea.Revive(2);
            }
            else if (flea.IsDead())
            {
                print("Can't revive flea since it's dead.");
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
                print("Can't reset flea since it's dead.");
            }
        }

        static GridFlea[] CreateGridFleas(int num)
        {
            GridFlea[] fleas = new GridFlea[num];

            for (int i = 0; i < fleas.Length; i++)
            {
                Random rand = new Random();
                int x = rand.Next(-500, 500);
                int y = rand.Next(-500, 500);
                uint size = (uint)rand.Next(0, 20);
                int reward = rand.Next(0, 1000);
                int energy = rand.Next(0, 25);

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
