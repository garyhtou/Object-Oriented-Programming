using DataExtractors;

public class DataPlus : DataExtractor
{
    public DataPlus(int[] x) : base(x)
    {
        a = xVals.First();
        k = xVals.Last();

        AddY(a, throwException: true);
    }

    public override int[] Target(uint z)
    {
        int[] output = Array.Empty<int>();

        foreach (int x in xVals)
        {
            if (x % 2 == 1)
            {
                output = output.Append(x).ToArray();
            }
        }

        foreach (int y in yVals)
        {
            if (y % 2 == 0)
            {
                output = output.Append(y).ToArray();
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

    private readonly int a;
    private int j = 1;
    private readonly int k;
}