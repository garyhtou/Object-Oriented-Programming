using DataExtractors;

public class DataHalf : DataExtractor
{
    public DataHalf(int[] x) : base(x)
    {
        FailureLimit = xVals.Last();

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

    protected override void BeforeRequest()
    {
        base.BeforeRequest();

        if (failedRequests >= FailureLimit)
        {
            MarkDeactivated();
        }
    }

    private readonly int FailureLimit;
    private int anyRequests = 0;
    private int[] previousAny;

    private bool ShouldNewAny()
    {
        return (new int[] { 0, 1 }).Contains(anyRequests % 4);
    }
}