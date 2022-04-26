namespace DataExtractors
{
    public enum State
    {
        Active,
        Invalid,
        Deactivated
    }

    public class DataExtractor
    {
        public DataExtractor(int[] x)
        {
            if (x.Length == 0)
            {
                markDeactivated();
            }

            xMinLength = xBaselineLength + (x.Length / 4);
            yMinLength = yBaselineLength + (x.Length / 2);

            foreach (int currX in x)
            {
                AddX(currX);
                AddY(currX * 2);
            }

            validateLengths();
        }

        public virtual int[] any()
        {
            beforeRequest();

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

        public virtual int[] target(uint z)
        {
            beforeRequest();

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

        public int sum(uint z)
        {
            beforeRequest();

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


        private int[] xVals;
        private int[] yVals = Array.Empty<int>();
        private static int xBaselineLength = 10;
        private static int yBaselineLength = 10;
        private int xMinLength;
        private int yMinLength;

        protected State state = State.Active;
        private int anyOffset = 0;

        protected int totalRequests = 0;
        protected int failedRequests = 0;

        protected virtual int[] GetXs()
        {
            return xVals;
        }

        protected virtual int[] GetYs()
        {
            return yVals;
        }

        protected void AddX(int x)
        {
            if (xVals.Contains(x))
            {
                markDeactivated();
            }

            xVals = xVals.Append(x).ToArray();
        }

        protected void AddY(int y)
        {
            if (yVals.Contains(y))
            {
                markDeactivated();
            }

            yVals = yVals.Append(y).ToArray();
        }

        protected virtual void beforeRequest()
        {
            totalRequests++;
        }

        private void validateLengths()
        {
            if (xVals.Length < xMinLength || yVals.Length < yMinLength)
            {
                state = State.Invalid;
            }
        }

        private void markInvalid()
        {
            failedRequests++;
            state = State.Invalid;
        }

        private void markDeactivated()
        {
            failedRequests++;
            state = State.Deactivated;
        }
    }
}