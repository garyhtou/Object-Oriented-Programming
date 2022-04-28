namespace DataExtractors;

public class DataHalf : DataExtractor
{
    public DataHalf(int[] x, uint xMinLength, uint yMinLength) : base(x, xMinLength, yMinLength)
    {
        failureLimit = xVals.Last();

        for (int i = 0; i < xVals.Length; i++)
        {
            xVals[i] /= 2;
        }
    }

    public override int[] Any()
    {
        if (ShouldNewAny())
        {
            previousAny = base.Any();
        }

        anyRequests++;
        return previousAny;
    }

    protected override void BeforeRequest(bool increment = true)
    {
        base.BeforeRequest();

        if (failedRequests >= failureLimit)
        {
            MarkDeactivated();
        }
    }

    private readonly int failureLimit;
    private int anyRequests = 0;
    private int[]? previousAny;

    private bool ShouldNewAny()
    {
        int rem = anyRequests % 4;
        return previousAny is null || rem is 0 or 1;
    }
}