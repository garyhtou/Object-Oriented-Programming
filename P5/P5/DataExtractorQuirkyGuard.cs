using DataExtractors;
using Guards;

namespace P5;

public class DataExtractorQuirkyGuard : DataGuard
{
    public DataExtractorQuirkyGuard(int[] x) :
        base(new DataExtractor(x), new QuirkyGuard(x))
    {
    }
}