namespace DataExtractors;

public class P3
{
    public static void Main(string[] args)
    {
    }


    private const uint NUM_CLASSES = 3;

    // =========================================================================
    // --------------------------- HELPER METHODS ------------------------------
    // =========================================================================
    private DataExtractor[] GetDataExtractors()
    {
        Random rnd = new Random();
        int len = rnd.Next(5, 30);
        DataExtractor[] extractors = new DataExtractor[len];

        for (int i = 0; i < len; i++)
        {
            int[] x = generateX();

            int option = rnd.Next((int)NUM_CLASSES);
            switch (option)
            {
                // case 1:
                //     extractors[i] = new DataExtractor(x);
                //     break;
                // case 2:
                //     extractors[i] = new DataHalf(x);
                //     break;
                // case 3:
                //     extractors[i] = new DataPlus(x);
                //     break;
            }
        }

        return extractors;
    }

    private int[] generateX()
    {
        Random rnd = new Random();
        int len = rnd.Next(1, 100);
        int[] x = new int[len];

        for (int i = 0; i < len; i++)
        {
            x[i] = rnd.Next(1, 100);
        }

        return x;
    }
}