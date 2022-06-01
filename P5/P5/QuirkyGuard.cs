namespace Guards;

public class QuirkyGuard : Guard
{
    public QuirkyGuard(int[] s) : base(s)
    {
        toggleLimit = (uint)sArray[0];
    }

    public override int Value(uint x)
    {
        // Replace
        Replace();

        // Concatenate
        AppendToArray(ref sArray, GetNewValue());

        return base.Value(x);
    }

    public override void Toggle()
    {
        if (toggleCount >= toggleLimit)
        {
            throw new Exception("Mode toggle limit reached");
        }

        base.Toggle();
        toggleCount++;
    }

    private readonly uint toggleLimit;
    private uint toggleCount = 0;

    private uint primeBase = 2;
    private uint nonPrimeBase = 5;

    private int GetNewValue()
    {
        return IsUp() ? GetPrime() : GetNonPrime();
    }

    private int GetPrime()
    {
        int max = 0;
        foreach (int val in sArray)
        {
            max = val > max ? val : max;
        }

        // Find next prime
        for (int i = max; i < max * 2; i++)
        {
            if (IsPrime(i)) return i;
        }

        return 3;
    }

    private int GetNonPrime()
    {
        return GetPrime() * 3;
    }

    private void Replace()
    {
        int index = (int)(toggleCount + primeBase + nonPrimeBase) %
                    sArray.Length;
        sArray[index] *= 2;
    }
}