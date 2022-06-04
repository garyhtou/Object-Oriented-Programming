// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// The Guard class is composed of a state represented by the "Mode". A Guard can
// either be UP or DOWN and starts as UP. The client can `toggle()` between
// modes to change the behavior of the object.
// Guard encapsulates an integer array s which is used by the `value()` method.
//
// It implements the IGuard interface. Error handling this class and its
// subtypes is done through throwing Exceptions.

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// `Guard()` (constructor):
//   - Requires an integer array, s, which can not be null and must contain at
//     least one element.
//   - Starts in UP mode
// `Value(uint x):
//   - `x` must be greater than or equal to 0. Enforced by the compiler via
//     `uint` data type.
//   - Returns a different value depending on the mode.
//   - Returns a `0` if it can not find a valid value (one which matches the
//     specifications)
// `Toggle()`:
//   - Toggles the current mode of the object (UP -> DOWN, DOWN -> UP).
// `IsUp()` and `IsDown()`:
//   - Query methods (returns a bool) to determine the current state of the
//     object.


namespace Guards;

public enum Mode
{
    UP,
    DOWN
}

public class Guard : IGuard
{
    // Pre: s array must not be null and must contain at least one element.
    // Post: none
    public Guard(int[] s)
    {
        if (s == null)
        {
            throw new Exception("s array can not be null");
        }

        if (s.Length <= 0)
        {
            throw new Exception("s array can not be null");
        }

        // Copy data from argument array
        sArray = new int[] { };
        foreach (var val in s)
        {
            AppendToArray(ref sArray, val);
        }
    }

    // Pre: none
    // Post: none
    public virtual int Value(uint x)
    {
        return ValueHelper(x, sArray);
    }

    // Pre: none
    // Post: none
    public virtual void Toggle()
    {
        mode = IsUp() ? Mode.DOWN : Mode.UP;
    }

    // Pre: none
    // Post: none
    public bool IsUp()
    {
        return mode == Mode.UP;
    }

    // Pre: none
    // Post: none
    public bool IsDown()
    {
        return mode == Mode.DOWN;
    }

    private Mode mode = Mode.UP;
    protected int[] sArray;

    protected int ValueHelper(uint x, int[] arr)
    {
        int prime = 0;
        bool first = true;
        for (int i = 0; i < arr.Length; i++)
        {
            int val = arr[i];
            // Console.WriteLine(val);
            if (
                (IsUp() && val <= x) ||
                (IsDown() && val >= x)
            )
            {
                continue;
            }

            if (!IsPrime(val)) continue;

            // Save the smallest/largest prime
            if (first ||
                (IsUp() && val < prime) ||
                (IsDown() && val > prime)
               )
            {
                prime = val;
                first = false;
            }
        }

        return prime;
    }

    protected static bool IsPrime(int val)
    {
        if (val <= 2) return false;

        for (int i = 2; i < val; i++)
        {
            if (val % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    protected static void AppendToArray(ref int[] array, int val)
    {
        int[] newArr = new int[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
        {
            newArr[i] = array[i];
        }

        newArr[array.Length] = val;

        array = newArr;
    }
}

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// Guard is the base class in the Guards class hierarchy. It contains the
// foundation necessary for SkipGuard and QuirkyGuard to build on. 
//
// >>> `Guard()` (Constructor) <<<
// The constructor checks for that the array passed in is not null and contains
// at least one element. Then, it will perform a copy of the injected array.
// 
// >>> `Value()` <<<
// The Value method is virtual to allow it to be overriden by descendent
// classes. It utilizes the `ValueHelper()` method which handles much of the
// computation to determining the value returned. This design was chosen as it
// allows descendent classes such as SkipGuard to easily pass in a different
// array to be used when computing the value while reusing the existing
// computation logic.
//
// >>> `ValueHelper()` <<<
// If in up mode, returns the smallest prime in s that is larger than x
// If in down mode, returns the largest prime in s that is smaller than x
// It utilizes the `IsPrime()` method for checking whether a number is prime or
// not.
//
// >>> `IsPrime()` <<<
// This helper method is mainly used byy `ValueHelper()` to determine whether a
// number is prime. It is static and protected to allow descendent classes to
// easily use it as well.
