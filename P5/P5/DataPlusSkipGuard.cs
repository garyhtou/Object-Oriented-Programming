// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// =============================================================================
// ----------------------------------- NOTE ------------------------------------
// =============================================================================
// Please see the DataGuard class for more information regarding the invariants
// and pre/post conditions.

using DataExtractors;
using Guards;

namespace P5;

public class DataPlusSkipGuard : DataGuard
{
    public DataPlusSkipGuard(int[] x) :
        base(new DataPlus(x), new SkipGuard(x))
    {
    }
}