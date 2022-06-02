using DataExtractors;
using Guards;

namespace P5;

public class DataPlusQuirkyGuard : DataGuard
{
    public DataPlusQuirkyGuard(int[] x) :
        base(new DataPlus(x), new QuirkyGuard(x))
    {
    }
}