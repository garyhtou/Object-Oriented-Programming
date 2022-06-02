using DataExtractors;
using Guards;

namespace P5;

public class DataExtractorGuard : DataGuard
{
    public DataExtractorGuard(int[] x) :
        base(new DataExtractor(x), new Guard(x))
    {
    }
}