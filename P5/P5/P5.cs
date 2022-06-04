// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

using Guards;

namespace P5;

public class P5
{
    private const int NUM_DATAGUARD_OBJS = 20;
    private const int NUM_GUARD_OBJS = 10;
    private const int NUM_DATAGUARD_CLASSES = 9;
    private const int NUM_GUARD_CLASSES = 3;

    private static DataGuard[] GetDataGuards(int n)
    {
        DataGuard[] gds = new DataGuard[n];
        for (int i = 0; i < gds.Length; i++)
        {
            int classOffset = rand.Next(0, NUM_DATAGUARD_CLASSES);
            int[] input = GetInputArray();
            switch (classOffset)
            {
                case 0:
                    gds[i] = new DataExtractorGuard(input);
                    break;
                case 1:
                    gds[i] = new DataExtractorSkipGuard(input);
                    break;
                case 2:
                    gds[i] = new DataExtractorQuirkyGuard(input);
                    break;
                case 3:
                    gds[i] = new DataHalfGuard(input);
                    break;
                case 4:
                    gds[i] = new DataHalfSkipGuard(input);
                    break;
                case 5:
                    gds[i] = new DataHalfQuirkyGuard(input);
                    break;
                case 6:
                    gds[i] = new DataPlusGuard(input);
                    break;
                case 7:
                    gds[i] = new DataPlusSkipGuard(input);
                    break;
                case 8:
                    gds[i] = new DataPlusQuirkyGuard(input);
                    break;
                default:
                    gds[i] = new DataExtractorGuard(input);
                    break;
            }
        }

        return gds;
    }

    private static IGuard[] GetGuards(int n)
    {
        IGuard[] gs = new IGuard[n];
        for (int i = 0; i < gs.Length; i++)
        {
            int classOffset = rand.Next(0, NUM_GUARD_CLASSES);
            int[] input = GetInputArray();
            switch (classOffset)
            {
                case 0:
                    gs[i] = new Guard(input);
                    break;
                case 1:
                    gs[i] = new SkipGuard(input);
                    break;
                case 2:
                    gs[i] = new QuirkyGuard(input);
                    break;
                default:
                    gs[i] = new Guard(input);
                    break;
            }
        }

        return gs;
    }

    private static int[] GetInputArray()
    {
        int len = rand.Next(15, 30);
        int[] arr = new int[len];

        int i = 0;
        while (i < len)
        {
            int newX = rand.Next(-20, 500);
            if (i == len - 1)
            {
                int lower = 3 < len - 1 ? 3 : len - 1;
                newX = rand.Next(lower, len - 1);
                if (newX % 2 == 0) continue;
            }

            if (ArrayContains(arr, newX)) continue;

            arr[i] = newX;
            i++;
        }

        return arr;
    }

    private static bool ArrayContains(int[] arr, int val)
    {
        foreach (int i in arr)
        {
            if (i == val) return true;
        }

        return false;
    }


    private static void RunGuards(IGuard[] guards)
    {
        for (int i = 0; i < guards.Length; i++)
        {
            int id = i + 1;
            IGuard g = guards[i];
            Console.WriteLine(
                $"------------ Running Guard #{id} ({g.GetType()}) -----------");

            for (int j = 0; j < 4; j++)
            {
                int val = g.Value(250);
                if (val != 0)
                {
                    Print("Value: " + val);
                }
                else
                {
                    Print("Value: No valid number found");
                }

                try
                {
                    g.Toggle();
                    Print("Toggled mode");
                }
                catch (Exception e)
                {
                    Print(e.Message);
                }
            }

            Console.WriteLine(
                $"------------------- Finished Running #{id}) ------------------\n");
        }
    }

    private static void RunDataGuards(DataGuard[] dataguards)
    {
        for (int i = 0; i < dataguards.Length; i++)
        {
            int id = i + 1;
            DataGuard dg = dataguards[i];
            Console.WriteLine(
                $"----------- Running DataGuard #{id} ({dg.GetType()}) ---------");

            try
            {
                Print("Any: " + ArrayToString(dg.Any()));
            }
            catch (Exception e)
            {
                Print("Any: " + e.Message);
            }

            try
            {
                Print("Target: " + ArrayToString(dg.Target(10)));
            }
            catch (Exception e)
            {
                Print("Target: " + e.Message);
            }

            try
            {
                Print("Sum: " + dg.Sum(10));
            }
            catch (Exception e)
            {
                Print("Sum: " + e.Message);
            }

            try
            {
                Print("Value: " + dg.Value(250));
            }
            catch (Exception e)
            {
                Print("Value: " + e.Message);
            }

            Console.WriteLine(
                $"------------------ Finished Running #{id}) -------------------\n\n");
        }
    }

    private static void Print(string message)
    {
        Console.WriteLine("  > " + message);
    }

    private static string ArrayToString(int[] arr)
    {
        return String.Join(", ", arr);
    }


    public static void Main(string[] args)
    {
        IGuard[] ds = GetGuards(NUM_GUARD_OBJS);
        Console.WriteLine(
            "===========================================================\n" +
            "---------------------- RUNNING GUARDS ---------------------\n" +
            "===========================================================\n");
        RunGuards(ds);

        Console.WriteLine(
            "\n" +
            "===========================================================\n" +
            "---------- RUNNING MULTIPLIED TYPES (DATAGUARDS) ----------\n" +
            "===========================================================\n");
        DataGuard[] dgs = GetDataGuards(NUM_DATAGUARD_OBJS);
        RunDataGuards(dgs);
    }

    private static readonly Random rand = new Random();
}