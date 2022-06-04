// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// The QuirkyGuard class inherits from Guard. Please see the Guard class for
// more information.
// The major different from Guard is that QuirkGuard may return different
// integer on subsequent calls to `value()` since it will concatenate numbers
// to the array. For more information, see the implementation invariant.
//
// It implements the IGuard interface. Error handling this class and its
// subtypes is done through throwing Exceptions.

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// `QuirkyGuard()` (constructor):
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
//   - You can only `Toggle()` an object a limited number of tines. It will
//     throw an exception when the limit is surpassed.
// `IsUp()` and `IsDown()`:
//   - Query methods (returns a bool) to determine the current state of the
//     object.


namespace Guards;

public class QuirkyGuard : Guard
{
    // Pre: s array must not be null and must contain at least one element.
    // Post: none
    public QuirkyGuard(int[] s) : base(s)
    {
        toggleLimit = (uint)sArray[0];
    }

    public override int Value(uint x)
    {
        // Replace
        Replace();

        // Concatenate
        AppendToArray(ref sArray, GetNewValue());

        return base.Value(x);
    }

    // Pre: none
    // Post: none
    public override void Toggle()
    {
        if (toggleCount >= toggleLimit)
        {
            throw new Exception("Mode toggle limit reached");
        }

        base.Toggle();
        toggleCount++;
    }

    private readonly uint toggleLimit;
    private uint toggleCount = 0;

    private uint primeBase = 2;
    private uint nonPrimeBase = 5;

    private int GetNewValue()
    {
        return IsUp() ? GetPrime() : GetNonPrime();
    }

    private int GetPrime()
    {
        int max = 0;
        foreach (int val in sArray)
        {
            max = val > max ? val : max;
        }

        // Find next prime
        for (int i = max; i < max * 2; i++)
        {
            if (IsPrime(i)) return i;
        }

        return 3;
    }

    private int GetNonPrime()
    {
        return GetPrime() * 3;
    }

    private void Replace()
    {
        int index = (int)(toggleCount + primeBase + nonPrimeBase) %
                    sArray.Length;
        sArray[index] *= 2;
    }
}

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// QuirkyGuard is-a Guard. Please see the Guard file for more information
// regarding this class.
//
// The major different from Guard is that QuirkGuard replaces numbers
// arbitrarily, and concatenates a prime number when in up mode, and a non-prime
// number when in down mode, upon each value query. quirkyGuards can switch
// modes only a limited number of times (variable across type but stable for an
// individual object).
//
// >>> `Value()` <<<
// Upon calling value, it will perform replacement by choosing an index to
// multiple by two. The integer in that index of the array is now replaced with
// the new value. The `replace()` method handles this logic.
// Afterwards, a new number is appending to the array. This uses the
// `GetNewValue()` method.
//
// >>> `GetNewValue()` <<<
// This method will return a prime number when in up mode and a non-prime number
// when in down mode. It utilizes the `IsPrime()` helper method to guess and
// check to find a prime. A non-prime is easily created by finding the next
// prime, then multiplying it by an integer (in this case, 3).
//
// >>> `Toggle()` <<<
// Toggle now has a limit. You can only toggle an object a limited number of
// times. This limit is determined by the first element in the array and is set
// by the constructor. Each time `Toggle()` is called, it increments a counter
// and checks it against the limit. If the number of calls surpasses the limit,
// an exception will be thrown.