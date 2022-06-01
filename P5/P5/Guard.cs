namespace Guards;

public enum Mode
{
    UP,
    DOWN
}

public class Guard
{
    public Guard(int[] s)
    {
        if (s == null)
        {
            throw new Exception("s array can not be null");
        }

        if (s.Length <= 0)
        {
            throw new Exception("s array can not be null");
        }

        // Copy data from argument array
        sArray = new int[] { };
        foreach (var val in s)
        {
            AppendToArray(ref sArray, val);
        }
    }

    public virtual int Value(uint x)
    {
        return ValueHelper(x, sArray);
    }

    public virtual void Toggle()
    {
        mode = IsUp() ? Mode.DOWN : Mode.UP;
    }

    public bool IsUp()
    {
        return mode == Mode.UP;
    }

    public bool IsDown()
    {
        return mode == Mode.DOWN;
    }

    private Mode mode = Mode.UP;
    protected int[] sArray;

    protected int ValueHelper(uint x, int[] arr)
    {
        int start = IsUp() ? 0 : arr.Length - 1;
        int end = IsUp() ? arr.Length : -1;
        int dir = IsUp() ? 1 : -1;

        int prime = 0;
        for (int i = start; i < end; i += dir)
        {
            int val = arr[i];
            if (
                (IsUp() && val <= x) ||
                (IsDown() && val >= x)
            )
            {
                continue;
            }

            if (!IsPrime(val)) continue;

            // Save the smallest/largest prime
            if (
                (IsUp() && val < prime) ||
                (IsDown() && val > prime)
            )
            {
                prime = val;
            }
        }

        return prime;
    }

    protected static bool IsPrime(int val)
    {
        if (val < 2) return false;

        for (int i = 2; i < val; i++)
        {
            if (val % i == 0)
            {
                return false;
            }
        }

        return true;
    }

    protected static void AppendToArray(ref int[] array, int val)
    {
        int[] newArr = new int[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
        {
            newArr[i] = array[i];
        }

        newArr[array.Length] = val;

        array = newArr;
    }
}