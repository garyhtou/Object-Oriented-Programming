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

    public int Value(uint x)
    {
        return guard.Value(x);
    }

    private readonly IData data;
    private readonly IGuard guard;
}