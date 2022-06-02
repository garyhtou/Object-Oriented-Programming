using DataExtractors;
using Guards;

namespace P5;

public class DataExtractorSkipGuard : DataGuard
{
    public DataExtractorSkipGuard(int[] x) :
        base(new DataExtractor(x), new SkipGuard(x))
    {
    }
}