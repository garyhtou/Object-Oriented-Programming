using Guards;

namespace P5;

public class P5
{
    private const int NUM_DATAGUARD_OBJS = 100;
    private const int NUM_DATAGUARD_CLASSES = 9;
    private const int NUM_GUARD_CLASSES = 3;

    private static DataGuard[] GetDataGuards(int n)
    {
        DataGuard[] gd = new DataGuard[n];
        for (int i = 0; i < gd.Length; i++)
        {
            int classOffset = rand.Next(0, NUM_DATAGUARD_CLASSES);
            int[] input = GetInputArray();
            switch (classOffset)
            {
                case 0:
                    gd[i] = new DataExtractorGuard(input);
                    break;
                case 1:
                    gd[i] = new DataExtractorSkipGuard(input);
                    break;
                case 2:
                    gd[i] = new DataExtractorQuirkyGuard(input);
                    break;
                case 3:
                    gd[i] = new DataHalfGuard(input);
                    break;
                case 4:
                    gd[i] = new DataHalfSkipGuard(input);
                    break;
                case 5:
                    gd[i] = new DataHalfQuirkyGuard(input);
                    break;
                case 6:
                    gd[i] = new DataPlusGuard(input);
                    break;
                case 7:
                    gd[i] = new DataPlusSkipGuard(input);
                    break;
                case 8:
                    gd[i] = new DataPlusQuirkyGuard(input);
                    break;
                default:
                    gd[i] = new DataExtractorGuard(input);
                    break;
            }
        }
    }

    private static int[] GetInputArray()
    {
        int len = rand.Next(5, 30);
        int[] arr = new int[len];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = rand.Next(-50, 50);
        }

        return arr;
    }

    private static IGuard[] GetGuards(int n)
    {
    }


    public static void Main(string[] args)
    {
        DataGuard[] dgs = GetDataGuards(NUM_DATAGUARD_OBJS);

        foreach (DataGuard dg in dgs)
        {
            dg.Any();
            dg.Target(10);
            dg.Sum(10);
            dg.Value(10);
        }
    }

    private static Random rand = new Random();
}