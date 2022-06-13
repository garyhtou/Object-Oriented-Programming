// Gary Tou
// April 29th, 2022
// CPSC 3200, P3

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// DataPlus is a class which takes in a 'x' integer array and provides
// methods for extracting data from the injected array. Over the lifetime of a
// DataPlus object, it can be either Active or Inactive.
//
// Please see the parent class (DataExtractor) for more information as a large
// portion of DataPlus's functionality is inherited from DataExtractor.
// 
// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// Please see the parent class (DataExtractor) for more information as a large
// portion of DataPlus's functionality is inherited from DataExtractor.
// The public interface of DataPlus is the same as DataExtractor.

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
        if (!AddY(a))
        {
            state = State.Inactive;
        }

        k = yVals.Last() + 1;
    }

    // Preconditions: none
    // Postconditions:
    //   - Active
    //   - Inactive (will throw Exception)
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

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// Please see the parent class (DataExtractor) for more information as a large
// portion of DataHalf's functionality is inherited from DataExtractor.
//
// However, there are some implementation differences between DataHalf and
// DataExtractor. They are listed below.
//
// >>> `DataPlus()` (constructor) <<<
// THe constructor will append a value `a` to the end of `yVals`. `a` is equal
// to the first value in `xVals`. Please note that this may result in the object
// being instantiated in an Inactive state if `a` already exists in `yVals`.
// However, this will not throw an exception. Please see DataExtractor's
// constructor Implementation Invariant for more information regarding
// constructors and exceptions.
//
// >>> `Target()` <<<
// The `Target()` method will now return an array with `z` odd values from
// `xVals` and `z` even values from `yVals`. As a result, it completely
// overrides DataExtractor's `Target()` method. However, it does continue to
// use the `BeforeRequest()` and `AppendToArray()` helper methods.
//
// >>> `BeforeRequest()` <<<
// The method has been overridden to provide additional state transitions.
// For every `k` requests, `j` (which starts at 1) is incremented and `j*k` is
// appended to `yVals`. It still calls the parent class's `BeforeRequest()` to
// maintain the state checks that were defined in the parent class.
