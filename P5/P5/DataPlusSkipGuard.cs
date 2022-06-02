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