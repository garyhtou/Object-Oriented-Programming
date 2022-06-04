// Gary Tou
// June 3rd, 2022
// CPSC 3200, P5

// =============================================================================
// ----------------------------- CLASS INVARIANTS ------------------------------
// =============================================================================
// This abstract class serves as a base class for all of the multiplied types.
// Please see the DataExtractor class and Guard classes for more information.

// =============================================================================
// --------------------------- INTERFACE INVARIANTS ----------------------------
// =============================================================================
// The methods available in this class is defined by the IData and IGuard
// interfaces. Please see the DataExtractor and Guard classes for more
// information.

using Guards;
using DataExtractors;

namespace P5;

public abstract class DataGuard : IData, IGuard
{
    protected DataGuard(IData data, IGuard guard)
    {
        this.data = data;
        this.guard = guard;
    }

    public int[] Any()
    {
        return data.Any();
    }

    public int[] Target(uint z)
    {
        return data.Target(z);
    }

    public int Sum(uint z)
    {
        return data.Sum(z);
    }

    public State GetState()
    {
        return data.GetState();
    }

    public bool IsActive()
    {
        return data.IsActive();
    }

    public bool IsInactive()
    {
        return data.IsInactive();
    }

    public int Value(uint x)
    {
        return guard.Value(x);
    }

    public void Toggle()
    {
        guard.Toggle();
    }

    private readonly IData data;
    private readonly IGuard guard;
}

// =============================================================================
// ------------------------- IMPLEMENTATION INVARIANTS -------------------------
// =============================================================================
// This class serves as a simple abstract class this echos methods from a object
// which implements the IData interface and another object that implements the
// IGuard interface. In other words, it encapsulated those two objects.
// The constructor of this class is protected (to be abstract) and takes in the
// IData and IGuard objects via constructor injection. Having already
// instantiated objects allow the descendant multiplied classes to be
// extremely tiny.