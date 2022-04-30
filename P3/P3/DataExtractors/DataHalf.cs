// Gary Tou
// April 29th, 2022
// CPSC 3200, P3

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// DataHalf is a class which takes in a 'x' integer array and provides
// methods for extracting data from the injected array. Over the lifetime of a
// DataHalf object, it can be either Active, Inactive, or Deactivated (a new
// state introduced by this class).
//
// This new `Deactivated` state occurs when the total number of invalid client
// requests exceeds a bound. Please see the DataExtractor class's Class
// Invariants for more information regarding invalid client requests.
//
// Please see the parent class (DataExtractor) for more information as a large
// portion of DataHalf's functionality is inherited from DataExtractor.
// 
// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// Please see the parent class (DataExtractor) for more information as a large
// portion of DataHalf's functionality is inherited from DataExtractor.
//
// The public interface of DataHalf is the same as DataExtractor except for the
// fact that `Any()`, `Target()` and `Sum()` may not be in a `Deactivated`
// state.

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

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// Please see the parent class (DataExtractor) for more information as a large
// portion of DataHalf's functionality is inherited from DataExtractor.
//
// However, there are some implementation differences between DataHalf and
// DataExtractor. They are listed below.
//
// >>> `DataHalf()` (constructor) <<<
// The constructor will divide every number in `xVals` by 2. In addition, it
// will set the `failureLimit` to the last number in `xVals`.
//
// >>> `Any()` <<<
// The `Any()` method now caches the result of the base class's `Any()` method
// and will return the cached results for every even number of `anyRequests`
// calls. The cached results are stored in a private variable `previousAny`.
// This provides the following functionality:
//   The 1st and 2nd `Any()` request return the same composite. The 3rd and 4th
//   `Any()` request return the same composite.
//
// >>> `MarkDeactivated()` and `IsDeactivated()` <<<
// DataHalf introduced an additional state: `Deactivated`. The `IsDeactivated`
// public query method adds to the existing state query functionality and allows
// the client and other methods to easily determine if the object is
// `Deactivated`. 
// The `MarkDeactivated()` method is used to mark the object as `Deactivated`.
// As with `MarkInactive()`, the `MarkDeactivated()` method will throw an
// Exception to alert the client of the state.
//
// >>> `BeforeRequest()` <<<
// The `BeforeRequest()` method is overridden to check if the number of failed
// client requests (`failedRequests`) is greater than the `failureLimit`. If so,
// the object will be marked as `Deactivated`. It still calls the parent class's
// `BeforeRequest()` to maintain the state checks that were defined in the
// parent class.
