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
        int[] output = { };

        foreach (int x in xVals)
        {
            if (x % 2 == 1)
            {
                output = AppendToArray(output, x);
            }
        }

        foreach (int y in yVals)
        {
            if (y % 2 == 0)
            {
                output = AppendToArray(output, y);
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