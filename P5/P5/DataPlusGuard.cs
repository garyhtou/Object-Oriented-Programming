using DataExtractors;
using Guards;

namespace P5;

public class DataPlusGuard : DataGuard
{
    public DataPlusGuard(int[] x) :
        base(new DataPlus(x), new Guard(x))
    {
    }
}