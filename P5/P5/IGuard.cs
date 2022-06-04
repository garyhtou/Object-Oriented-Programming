// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// =============================================================================
// ----------------------------------- NOTE ------------------------------------
// =============================================================================
// Please see the Guard class for more information regarding the
// invariants and pre/post conditions.

namespace Guards;

public interface IGuard
{
    public int Value(uint x);

    public void Toggle();
}