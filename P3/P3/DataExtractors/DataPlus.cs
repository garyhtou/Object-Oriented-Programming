namespace DataExtractors;

public class DataPlus : DataExtractor
{
    // Preconditions:
    //   - Length of `x` must be greater than 0.
    //   - `xMinLength` must be greater than 0.
    //   - `yMinLength` must be greater than 0.
    // Postconditions:
    //   - Active
    //   - Inactive (if `x` contains duplicate elements, or if `x` and `y`
    //     arrays are shorter than their respective `MinLength`)
    public DataPlus(int[] x, uint xMinLength, uint yMinLength) : base(x, xMinLength, yMinLength)
    {
        int a = xVals.First();
        AddY(a, throwException: true);

        k = yVals.Last() + 1;
    }

    // Preconditions: none
    // Postconditions:
    //   - Active
    //   - Inactive (will throw Exception)
    //   - Deactivated (will throw Exception)
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
            AddY(j * k, throwException: true);
            j++;
        }
    }

    private int j = 1;
    private readonly int k;
}