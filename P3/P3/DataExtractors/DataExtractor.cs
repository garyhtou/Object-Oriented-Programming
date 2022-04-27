namespace DataExtractors
{
    public enum State
    {
        Active,
        Deactivated
    }

    public class DataExtractor
    {
        public DataExtractor(int[] x)
        {
            if (x.Length == 0)
            {
                state = State.Deactivated;
                throw new Exception("x array must not be empty");
            }

            xMinLength = xBaselineLength + (x.Length / 4);
            yMinLength = yBaselineLength + (x.Length / 2);

            foreach (int currX in x)
            {
                if (!AddX(currX) || !AddY(currX * 2))
                {
                    state = State.Deactivated;
                }
            }

            if (HasInvalidLengths())
            {
                state = State.Deactivated;
            }
        }

        public virtual int[] Any()
        {
            BeforeRequest();

            bool toggle = true;
            int minLen = Math.Min(xVals.Length, yVals.Length);

            int[] composite = Array.Empty<int>();

            // Alternate adding elements from both array
            for (int i = 0; i < minLen; i++)
            {
                int valsIndex = (i + anyOffset);
                if (toggle)
                {
                    composite[i] = xVals[valsIndex % xVals.Length];
                }
                else
                {
                    composite[i] = yVals[valsIndex % yVals.Length];
                }

                toggle = !toggle;
            }

            anyOffset++;
            return composite;
        }

        public virtual int[] Target(uint z)
        {
            BeforeRequest();

            bool even = totalRequests % 2 == 0;
            int[] output = Array.Empty<int>();

            foreach (int x in xVals)
            {
                if (x % 2 == (even ? 0 : 1))
                {
                    output[x] = x;
                }
            }

            return output;
        }

        public int Sum(uint z)
        {
            BeforeRequest();

            bool even = totalRequests % 2 == 0;
            int output = 0;

            foreach (int y in yVals)
            {
                if (y % 2 == (even ? 0 : 1))
                {
                    output += y;
                }
            }

            return output;
        }


        protected int[] xVals;
        protected int[] yVals = Array.Empty<int>();

        private static int xBaselineLength = 10;
        private static int yBaselineLength = 10;
        private int xMinLength;
        private int yMinLength;

        protected State state = State.Active;
        protected int totalRequests = 0;
        protected int failedRequests = 0;

        private int anyOffset = 0;

        protected bool isActive()
        {
            return state == State.Active;
        }

        protected bool IsDeactivated()
        {
            return state == State.Deactivated;
        }

        protected void MarkDeactivated()
        {
            failedRequests++;
            state = State.Deactivated;
            throw new Exception("Invalid request");
        }

        protected bool AddX(int x, bool throwException = false)
        {
            if (xVals.Contains(x))
            {
                if (throwException) throw new Exception($"{x} already exists in 'x' array");

                return false;
            }

            xVals = xVals.Append(x).ToArray();
            return true;
        }

        protected bool AddY(int y, bool throwException = false)
        {
            if (yVals.Contains(y))
            {
                if (throwException) throw new Exception($"{y} already exists in 'y' array");

                return false;
            }

            yVals = yVals.Append(y).ToArray(); // TODO: is it okay to use this?
            return true;
        }

        protected virtual void BeforeRequest()
        {
            totalRequests++;
            if (IsDeactivated())
            {
                failedRequests++;
                throw new Exception("Invalid request");
            }
        }

        private bool HasInvalidLengths()
        {
            return xVals.Length < xMinLength || yVals.Length < yMinLength;
        }
    }
}