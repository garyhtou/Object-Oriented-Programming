// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// =============================================================================
// ----------------------------------- NOTE ------------------------------------
// =============================================================================
// Please see the DataExtractor class for more information regarding the
// invariants and pre/post conditions.

namespace DataExtractors;

public interface IData
{
    public int[] Any();
    public int[] Target(uint z);
    public int Sum(uint z);

    public State GetState();

    public bool IsActive();
    public bool IsInactive();
}