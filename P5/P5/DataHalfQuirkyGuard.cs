using DataExtractors;
using Guards;

namespace P5;

public class DataHalfQuirkyGuard : DataGuard
{
    public DataHalfQuirkyGuard(int[] x) :
        base(new DataHalf(x), new QuirkyGuard(x))
    {
    }
}