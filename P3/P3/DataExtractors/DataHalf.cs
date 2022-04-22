using DataExtractors;

public class DataHalf : DataExtractor
{
    public DataHalf(int[] x) : base(x)
    {
        FailureLimit = x.Last();
    }

    protected override int[] GetXs()
    {
        int[] halved = base.GetXs();
        for (int i = 0; i < halved.Length; i++)
        {
            halved[i] /= 2;
        }

        return halved;
    }


    protected readonly int FailureLimit;

    protected override void beforeRequest()
    {
        base.beforeRequest();
        if (failedRequests >= FailureLimit)
        {
            throw new InvalidOperationException("Too many failed requests");
        }
    }
}