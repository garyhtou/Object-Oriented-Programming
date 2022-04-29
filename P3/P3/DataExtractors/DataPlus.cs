namespace DataExtractors;

public class DataPlus : DataExtractor
{
    public DataPlus(int[] x, uint xMinLength, uint yMinLength) : base(x, xMinLength, yMinLength)
    {
        k = xVals.Last();

        int a = xVals.First();
        AddY(a, throwException: true);
    }

    public override int[] Target(uint z)
    {
        BeforeRequest();

        int[] output = { };
        int numZ = 0;

        foreach (int x in xVals)
        {
            if (numZ == z) break;
            if (x % 2 == 1)
            {
                AppendToArray(ref output, x);
                numZ++;
            }
        }

        numZ = 0;
        foreach (int y in yVals)
        {
            if (numZ == z) break;
            if (y % 2 == 0)
            {
                AppendToArray(ref output, y);
                numZ++;
            }
        }

        return output;
    }

    protected override void BeforeRequest()
    {
        base.BeforeRequest();

        if (totalRequests % k == 0)
        {
            j++;
        }

        if (totalRequests == j * k)
        {
            AddY(totalRequests, throwException: true);
        }
    }

    private int j = 1;
    private readonly int k;
}