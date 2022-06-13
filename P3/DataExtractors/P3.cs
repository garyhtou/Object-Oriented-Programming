namespace DataExtractors;

public class P3
{
    private const string PRINT_PREFIX = "|  > ";
    private const int NUM_OBJECTS = 15;
    private const int CALLS_NUM_TIMES = 20;
    private const int Z_LIMIT = 5;
    private const int X_MAX_VALUE = 100;
    private const int X_LAST_MAX_VALUE = X_MAX_VALUE / 2;
    private const int X_MAX_LENGTH = 30;
    private const int X_LAST_REDUCTION_FACTOR = 3;

    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the DataExtractor Driver!\n");
        DataExtractor[] de = GenerateDataExtractors();
        for (int i = 0; i < de.Length; i++)
        {
            Run(de[i], i);
        }
    }

    private static void Run(DataExtractor d, int index)
    {
        index++;
        string type = d.GetType().Name;
        string idType = $"#{index} ({type})";
        Console.WriteLine($"======= Running {idType} =======");
        Print($"This {type} was created as {d.GetState()}");

        CallAny(d, CALLS_NUM_TIMES);
        CallTarget(d, CALLS_NUM_TIMES);
        CallSum(d, CALLS_NUM_TIMES);

        Print($"This {type} is currently {d.GetState()}");
        Console.WriteLine($"=== Finished Running {idType} ===\n\n");
    }


    private static void CallAny(DataExtractor d, int times = 1)
    {
        State initState = d.GetState();
        int i = 0;
        while (i < times)
        {
            i++;
            try
            {
                d.Any();
            }
            catch (Exception)
            {
                // NOP, ignored
            }

            if (d.GetState() != initState)
            {
                Print(
                    $"This {d.GetType().Name} changed from {initState} to {d.GetState()} after calling `Any()` {i} times.");
                return;
            }
        }

        Print($"Called `Any()` {i} times.");
    }


    private static void CallTarget(DataExtractor d, int times = 1)
    {
        Random rnd = new Random();
        State initState = d.GetState();
        int i = 0;
        while (i < times)
        {
            i++;
            uint z = (uint)rnd.Next(1, Z_LIMIT);
            try
            {
                d.Sum(z);
            }
            catch (Exception)
            {
                // NOP, ignored
            }

            if (d.GetState() != initState)
            {
                Print(
                    $"This {d.GetType().Name} changed from {initState} to {d.GetState()} after calling `Target()` {i} times.");
                return;
            }
        }

        Print($"Called `Target()` {i} times.");
    }

    private static void CallSum(DataExtractor d, int times = 1)
    {
        Random rnd = new Random();
        State initState = d.GetState();
        int i = 0;
        while (i < times)
        {
            i++;
            uint z = (uint)rnd.Next(1, Z_LIMIT);
            try
            {
                d.Sum(z);
            }
            catch (Exception)
            {
                // NOP, ignored
            }

            if (d.GetState() != initState)
            {
                Print(
                    $"This {d.GetType().Name} changed from {initState} to {d.GetState()} after calling `Sum()` {i} times.");
                return;
            }
        }

        Print($"Called `Sum()` {i} times.");
    }

    // =========================================================================
    // --------------------------- HELPER METHODS ------------------------------
    // =========================================================================
    enum DataClass
    {
        DataExtractor,
        DataHalf,
        DataPlus
    }

    private static DataExtractor[] GenerateDataExtractors()
    {
        Random rnd = new Random();
        int len = NUM_OBJECTS;
        DataExtractor[] extractors = new DataExtractor[len];

        for (int i = 0; i < len; i++)
        {
            DataClass klass = RandomDataClass();
            bool forceValid = rnd.Next(0, 2) == 1;
            try
            {
                extractors[i] = GenerateDataObject(klass, forceValid);
            }
            catch (Exception)
            {
                // Try again
                i--;
            }
        }

        return extractors;
    }

    private static DataClass RandomDataClass()
    {
        Random rnd = new Random();

        int numClasses = Enum.GetNames(typeof(DataClass)).Length;
        int rndClass = rnd.Next(numClasses);

        switch (rndClass)
        {
            case 0:
                return DataClass.DataExtractor;
            case 1:
                return DataClass.DataHalf;
            case 2:
                return DataClass.DataPlus;
            default:
                throw new NotImplementedException();
        }
    }


    private static DataExtractor GenerateDataObject(DataClass type, bool forceValid = true)
    {
        // Only generates valid objects
        Random rnd = new Random();
        int len = rnd.Next(1, X_MAX_LENGTH);
        int[] xArray = new int[len];

        int i = 0;
        while (i < len)
        {
            int upper = i == len - 1 ? X_LAST_MAX_VALUE : X_MAX_VALUE;
            int newX = rnd.Next(1, upper);
            if (forceValid && ArrayContains(xArray, newX)) continue;

            xArray[i] = newX;
            i++;
        }

        if (!forceValid)
        {
            // Introduce chance of min length being longer than array length
            len += rnd.Next(len / X_LAST_REDUCTION_FACTOR);
        }

        uint xMinLength = (uint)rnd.Next(1, len);
        uint yMinLength = (uint)rnd.Next(1, len);

        switch (type)
        {
            case DataClass.DataExtractor:
                return new DataExtractor(xArray, xMinLength, yMinLength);
            case DataClass.DataHalf:
                return new DataHalf(xArray, xMinLength, yMinLength);
            case DataClass.DataPlus:
                return new DataPlus(xArray, xMinLength, yMinLength);
            default:
                throw new NotImplementedException("Unknown class type");
        }
    }

    private static bool ArrayContains(int[] arr, int val)
    {
        foreach (int i in arr)
        {
            if (i == val) return true;
        }

        return false;
    }

    private static void Print(string message)
    {
        Console.WriteLine(PRINT_PREFIX + message);
    }
}