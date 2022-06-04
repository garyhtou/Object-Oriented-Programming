// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// Revision History:
// - May 31 (P5). dataExtractor’s minimum length is dependent (in some manner)
//   on the value of the last odd number of its encapsulated sequence.

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// DataExtractor is a class which takes in a 'x' integer array and provides
// methods for extracting data from the injected array. Over the lifetime of a
// DataExtractor object, it can be either Active or Inactive.
// 
// A DataExtractor is Active when all three of the following are true:
//   - The length of the `x` array is greater than or equal to the `xMinLength`.
//   - The length of the `y` array is less than or equal to the `yMaxLength`.
//   - Both the `x` and `y` arrays do not individually contain duplicate values.
//     They each function as a "set" of unique values.
// `xMinLength` and `yMinLength` is the last odd number in the `x` array.
// Otherwise, the DataExtractor is Inactive.
// In addition, the possible States of a DataExtractor, and it's descendants, is
// stored in the `State` enum defined in the `DataExtractors` namespace. Please
// not that it is not defined within the `DataExtractor` class itself.
//
// The `Any()`, `Target()`, and `Sum()` methods of DataExtractor require that
// the DataExtractor is Active. Calls to a non-Active DataExtractor will be
// considered as an "invalid client request" and will result in a state change
// with an Exception being thrown.
//
// DataExtractor implements in the IData interface.
// Error handling for DataExtractor and its subtypes is done through throwing
// Exceptions.

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// `DataExtractor()` (constructor):
//   - Requires Length of `x` integer array to be greater than 0.
//   - Requires `xMinLength` to be greater than 0.
//   - Requires `xMaxLength` to be greater than 0.
//   - Instantiates a new DataExtractor object as Active or Inactive. It will be
//     Active is the Length of both arrays are greater than their respective
//     minimum lengths. Otherwise, it will be Inactive.
// `Any()`:
//   - Requires the DataExtractor to be in an Active state. Otherwise, it will 
//     be considered as an invalid client request and an Exception will be
//     thrown.
//   - Returns an integer array composed of values from the `x` array. The
//     returned array will differ between calls to `Any()`.
// `Target(int z)`:
//   - Requires the DataExtractor to be in an Active state. Otherwise, it will 
//     be considered as an invalid client request and an Exception will be
//     thrown.
//   - Returns an integer array composed of either all odd or all even values
//     from the `x` array. Whether the returned array is all odd or all even
//     will be determined by `z`. If `z` is even, the returned array will also
//     be even. If `z` is odd, the returned array will also be odd. The returned
//     array will be of length `z`.
// `Sum()`:
//   - Requires the DataExtractor to be in an Active state. Otherwise, it will 
//     be considered as an invalid client request and an Exception will be
//     thrown.
//   - Returns the sum of a values where all values are either odd or all even
//     from the `x` array. The values to sum are determined using similar logic
//     to the `Target()` method.
//
// In addition, DataExtractor provides multiple query methods for determining
// the state of the object. These methods maybe called regardless of the current
// state of the object.
// `GetState()`:
//   - Returns the current state of the DataExtractor. This will be a value from
//     the `State` enum mentioned above in the Class Invariants section.
// `IsActive()`:
//   - Returns true if the DataExtractor is in an Active state. Otherwise, it
//     will return false. This serves as syntactic sugar for the `GetState()`.
// `IsInactive()`:
//   - Returns true if the DataExtractor is in an Inactive state. Otherwise, it
//     will return false. This serves as syntactic sugar for the `GetState()`.

namespace DataExtractors
{
    public enum State
    {
        Active,
        Inactive,
        Deactivated
    }

    public class DataExtractor : IData
    {
        // Preconditions:
        //   - Length of `x` must be greater than 0.
        //   - `xMinLength` must be greater than 0.
        //   - `yMinLength` must be greater than 0.
        // Postconditions:
        //   - Active
        //   - Inactive (if `x` contains duplicate elements, or if `x` and `y`
        //     arrays are shorter than their respective `MinLength`)
        public DataExtractor(int[] x)
        {
            if (x == null)
            {
                throw new Exception("x array must not be null");
            }

            if (x.Length == 0)
            {
                throw new Exception("x array must not be empty");
            }

            foreach (int currX in x)
            {
                if (!AddX(currX) || !AddY(currX * 2))
                {
                    state = State.Inactive;
                }
            }

            if (HasInvalidLengths())
            {
                state = State.Inactive;
            }
        }

        // Preconditions: DataExtractor is Active.
        // Postconditions:
        //   - Active
        //   - Inactive (will throw Exception)
        public virtual int[] Any()
        {
            BeforeRequest();

            return AnyHelper();
        }


        // Preconditions: DataExtractor is Active.
        // Postconditions:
        //   - Active
        //   - Inactive (will throw Exception)
        public virtual int[] Target(uint z)
        {
            BeforeRequest();

            return TargetHelper(z);
        }

        // Preconditions: DataExtractor is Active.
        // Postconditions:
        //   - Active
        //   - Inactive (will throw Exception)
        public int Sum(uint z)
        {
            BeforeRequest();

            int[] target = TargetHelper(z);

            int output = 0;
            foreach (int val in target)
            {
                output += val;
            }

            return output;
        }


        // Preconditions: DataExtractor is Active.
        // Postconditions: none
        public State GetState()
        {
            return state;
        }

        // Preconditions: DataExtractor is Active.
        // Postconditions: none
        public bool IsActive()
        {
            return state == State.Active;
        }

        // Preconditions: none
        // Postconditions: none
        public bool IsInactive()
        {
            return state == State.Inactive;
        }

        protected int[] xVals = { };
        protected int[] yVals = { };

        protected State state = State.Active;
        protected int totalRequests = 0;
        protected int failedRequests = 0;

        private int anyOffset = 0;

        protected void MarkInactive()
        {
            failedRequests++;
            if (IsActive())
            {
                state = State.Inactive;
            }

            throw new Exception("Object is InActive");
        }

        private bool AddX(int x, bool throwException = false)
        {
            if (ArrayContains(xVals, x))
            {
                if (throwException)
                    throw new Exception($"{x} already exists in 'x' array");

                return false;
            }

            AppendToArray(ref xVals, x);
            return true;
        }

        protected bool AddY(int y, bool throwException = false)
        {
            if (ArrayContains(yVals, y))
            {
                if (throwException)
                    throw new Exception($"{y} already exists in 'y' array");

                return false;
            }

            AppendToArray(ref yVals, y);
            return true;
        }

        protected int[] AnyHelper()
        {
            bool toggle = true;
            int minLen = Math.Min(xVals.Length, yVals.Length);

            int[] composite = { };

            // Alternate adding elements from both array
            for (int i = 0; i < minLen; i++)
            {
                int valsIndex = (i + anyOffset);
                int newVal;
                if (toggle)
                {
                    newVal = xVals[valsIndex % xVals.Length];
                }
                else
                {
                    newVal = yVals[valsIndex % yVals.Length];
                }

                AppendToArray(ref composite, newVal);
                toggle = !toggle;
            }

            anyOffset++;
            return composite;
        }

        private int[] TargetHelper(uint z)
        {
            bool even = z % 2 == 0;
            int[] output = { };

            int[] arr = totalRequests % 2 == 0 ? xVals : yVals;

            for (int i = 0; i < arr.Length; i++)
            {
                if (output.Length == z)
                {
                    break;
                }

                if (arr[i] % 2 == (even ? 0 : 1))
                {
                    AppendToArray(ref output, arr[i]);
                }
            }

            return output;
        }

        protected virtual void BeforeRequest()
        {
            totalRequests++;

            if (IsInactive())
            {
                failedRequests++;
                throw new Exception("Object is InActive");
            }
        }

        private bool HasInvalidLengths()
        {
            return xVals.Length < GetXMinLength() ||
                   yVals.Length < GetYMinLength();
        }

        protected static void AppendToArray(ref int[] arr, int val)
        {
            int[] newArr = new int[arr.Length + 1];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i];
            }

            newArr[arr.Length] = val;
            arr = newArr;
        }

        private static bool ArrayContains(int[] arr, int val)
        {
            foreach (int i in arr)
            {
                if (i == val) return true;
            }

            return false;
        }

        protected uint GetXMinLength()
        {
            return GetMinLength(xVals);
        }

        protected uint GetYMinLength()
        {
            return GetMinLength(yVals);
        }

        private uint GetMinLength(int[] array)
        {
            for (int i = array.Length - 1; i >= 0; i--)
            {
                // Since the arrays may contain negatives, I return the
                // absolute value.
                if (i % 2 == 1) return (uint)Math.Abs(array[i]);
            }

            return 0;
        }
    }
}

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// A DataExtractor's overall state is dependent on on following
// private/protected variables:
//   - `xVals`. An array of integers that is dependency injected via the
//     constructor.
//   - `yVals`. An array of integers that is generated using `xVals`.
//   - `xMinLength`. An integer that is passed to the constructor; representing
//     the minimum length that `xVals` must be in order to be considered Active.
//   - `yMinLength`. An integer that is passed to the constructor; representing
//     the minimum length that `yVals` must be in order to be considered Active.
//   - `totalRequests`. An integer that is incremented every time a request is
//      made to `Any()`, `Target()`, or `Sum()`.
//   - `failedRequests`. An integer that is incremented every time an "invalid
//      client request" is made. Please see the Class Invariant for more
//      information regarding the definition of "invalid client request".
//   -  `anyOffset`. An integer that is incremented every time `Any()` is
//      called. This is used to help `Any()` generate varying outputs for each
//      call.
// The culmination of all of these variables will results in the State of the
// DataExtractor being with Active or Inactive. This State is stored in the
// `state` variable and will be a value from the `State` enum. Please see the
// Class Invariant for more information regarding the `State` enum.
//
// >>> `DataExtractor()` (Constructor) <<<
// The constructor takes `int[] x` (dependency injected) and uses it to generate
// an array for `yVals`. All parameters are first checked for validity (if
// necessary). If invalid parameters are passed, an exception is thrown.
// For simplicity, `yVals` (y array) is generated by doubling the values in
// `xVals` (which was dpenedency injected as `x`). The constructor, as well as
// many other methods, utilize helper methods for adding to `xVals` and `yVals`.
// Please see the "AddX() and AddY()" section in this Implementation Invariant
// for more information. Last but not least, the constructor will use an
// additional helper method, `HasInvalidLengths()`, to verify that the length
// of `xVals` and `yVals` are greater than or equal to `xMinLength` and
// `yMinLength` respectively.
// No exceptions are throw by the constructor for valid object construction that
// results in a non-Active (Inactive) state. Instead, the object will be
// instantiated in that non-Active state. This is to allow for creation of
// objects that vary in state.
//
// >>> `Any()` <<<
// The `Any()` method utilizes a shared virtual helper method, `BeforeRequest()`
// which handles the logic of request tracking and state checking. Please see
// the "BeforeRequest()" section in this Implementation Invariant for more
// information.
// The `Any()` method will return an integer array composed of values from both
// `xVals` and `yVals`. The source of the values alternates between `xVals` and
// `yVals` for each element in the returned array. In addition, the resulting
// array will change over time as `anyOffset` is incremented per request and is
// used to offset the initial index of the source arrays. This array generation
// logic is encapsulated in the `AnyHelper()` method to allow `BeforeRequest()`
// to be called only when `Any()` is called by the client.
//
// >>> `Target()` <<<
// The `Target()` method utilizes a shared virtual helper method,
// `BeforeRequest()`. Please see the "BeforeRequest()" section in this
// Implementation Invariant for more information.
// The `Target()` method will return an integer array composed of values from
// `xVals`, and will be either all odd or all even values. Whether the values
// are odd or even is determined by `z` (passed as a parameter). If `z` is
// odd, then the returned array will be composed of odd values from `xVals`.
// Otherwise, the returned array will be composed of only even values.
//
// >>> `Sum()` <<<
// The `Sum()` method utilizes a shared virtual helper method,
// `BeforeRequest()`. Please see the "BeforeRequest()" section in this
// Implementation Invariant for more information.
// The `Sum()` method returns the sum of all values resulting from a call to the
// `TargetHelper()` method. The `TargetHelper()` method handles the logic of
// picking values which are either all odd or all even. Afterwards, the values
// are summed and returned.
//
// >>> State Management <<<
// The class contains protected methods such as `MarkInactive()` which will
// mark the object as Inactive. As well as increment the number of failed
// requests (`failedRequests`).
//
// >>> `AddX()` and `AddY()` <<<
// These methods simplify append a value to the `xVals` or `yVals` array. They
// utilize the `AppendToArray()` method to handle the logic of extending the
// length of the extending array and managing memory. It will also verify that
// no duplicate values are added to the array. Exceptions will be thrown if the
// client attempts to add a duplicate value.
//
// >>> `BeforeRequest()` <<<
// The `BeforeRequest()` method is a shared virtual helper method which is
// utilized by `Any()`, `Target()`, and `Sum()` methods. It handles the logic
// of tracking the number of requests, and the state of the object. This method
// is virtual to allow descendants to override and provide additional state
// management logic