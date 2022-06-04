// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// The SkipGuard class inherits from Guard. Please see the Guard class for more
// information.
// The major different from Guard is that SkipGuard skips k numbers in s when
// computing `value(x)` where k is an unstable number that varies from object to
// object. `k` will change upon every call to `value(x)`.
//
// It implements the IGuard interface. Error handling this class and its
// subtypes is done through throwing Exceptions.

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// `SkipGuard()` (constructor):
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

public class SkipGuard : Guard
{
    // Pre: s array must not be null and must contain at least one element.
    // Post: none
    public SkipGuard(int[] s) : base(s)
    {
    }

    // Pre: none
    // Post: none
    public override int Value(uint x)
    {
        int k = GetK();

        int[] kArray = { };
        for (int i = 0; i < sArray.Length; i++)
        {
            if (i % k == 0) continue;
            AppendToArray(ref kArray, sArray[i]);
        }

        return ValueHelper(x, kArray);
    }


    private int kIndex = 0;

    private int GetK()
    {
        kIndex++;
        return Math.Abs(sArray[kIndex % sArray.Length]);
    }
}

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// SkipGuard is-a Guard. Please see the Guard file for more information
// regarding this class.
//
// The major different from Guard is that SkipGuard skips k numbers in s when
// computing `value(x)` where k is an unstable number that varies from object to
// object. `k` will change upon every call to `value(x)`.
//
// >>> `Value()` <<<
// Value now will create a new temp array which skips every k element, then pass
// that array to `ValueHelper()` from the base class. The value from the helper
// method is then returned. This helper method also for maximum code reuse as
// there is no longer a need to rewrite the logic for choosing a value form an
// array. K is determined using the `GetK()` method.
//
// >>> `GetK()` <<<
// Since K is unstable, it is not stored as a member data variable. Instead, it
// is generated on the fly by calling this method. This method will increment a
// counter everytime k is gotten, and uses the counter to rotate around elements
// in the array. The absolute value of the current element is then chosen to be
// the value k.
