using DataExtractors;
using Guards;

namespace P5;

public class DataHalfSkipGuard : DataGuard
{
    public DataHalfSkipGuard(int[] x) :
        base(new DataHalf(x), new SkipGuard(x))
    {
    }
}