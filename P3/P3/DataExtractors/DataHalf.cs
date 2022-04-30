namespace DataExtractors;

public class DataHalf : DataExtractor
{
    // Preconditions:
    //   - Length of `x` must be greater than 0.
    //   - `xMinLength` must be greater than 0.
    //   - `yMinLength` must be greater than 0.
    // Postconditions:
    //   - Active
    //   - Inactive (if `x` contains duplicate elements, or if `x` and `y`
    //     arrays are shorter than their respective `MinLength`)
    public DataHalf(int[] x, uint xMinLength, uint yMinLength) : base(x, xMinLength, yMinLength)
    {
        failureLimit = xVals.Last();

        for (int i = 0; i < xVals.Length; i++)
        {
            xVals[i] /= 2;
        }
    }

    // Preconditions: none
    // Postconditions:
    //   - Active
    //   - Inactive (will throw Exception)
    //   - Deactivated (will throw Exception)
    public override int[] Any()
    {
        anyRequests++;
        BeforeRequest();

        if (ShouldNewAny() || previousAny is null)
        {
            previousAny = base.AnyHelper();
        }

        return previousAny;
    }

    // Preconditions: none
    // Postconditions: none
    public bool IsDeactivated()
    {
        return state == State.Deactivated;
    }

    private void MarkDeactivated()
    {
        failedRequests++;
        state = State.Deactivated;
        throw new Exception("Invalid request. Object is Deactivated");
    }

    protected override void BeforeRequest()
    {
        totalRequests++;

        if (failedRequests >= failureLimit)
        {
            MarkDeactivated();
        }

        if (IsInactive())
        {
            MarkInactive();
        }
    }

    private readonly int failureLimit;
    private int anyRequests = 0;
    private int[]? previousAny;

    private bool ShouldNewAny()
    {
        return anyRequests % 2 == 1;
    }
}