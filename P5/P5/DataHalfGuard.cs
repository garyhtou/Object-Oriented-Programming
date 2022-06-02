using DataExtractors;
using Guards;

namespace P5;

public class DataHalfGuard : DataGuard
{
    public DataHalfGuard(int[] x) :
        base(new DataHalf(x), new Guard(x))
    {
    }
}