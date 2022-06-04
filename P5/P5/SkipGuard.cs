namespace Guards;

public class SkipGuard : Guard
{
    public SkipGuard(int[] s) : base(s)
    {
    }

    public override int Value(uint x)
    {
        int k = GetK();

        int[] kArray = { };
        for (int i = 0; i < sArray.Length; i++)
        {
            if (i % k == 0) continue;
            AppendToArray(ref kArray, sArray[i]);
        }

        return ValueHelper(x, kArray);
    }


    private int kIndex = 0;

    private int GetK()
    {
        kIndex++;
        return Math.Abs(sArray[kIndex % sArray.Length]);
    }
}