namespace DataExtractors
{
    public enum State
    {
        Active,
        Deactivated
    }

    public class DataExtractor
    {
        public DataExtractor(int[] x, uint xMinLength, uint yMinLength)
        {
            if (x.Length == 0)
            {
                throw new Exception("x array must not be empty");
            }

            if (xMinLength == 0)
            {
                throw new Exception("xMinLength must not be zero");
            }

            if (yMinLength == 0)
            {
                throw new Exception("yMinLength must not be zero");
            }

            this.xMinLength = (int)xMinLength;
            this.yMinLength = (int)yMinLength;

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

            int[] composite = { };

            // Alternate adding elements from both array
            for (int i = 0; i < minLen; i++)
            {
                int valsIndex = (i + anyOffset);
                int newVal;
                if (toggle)
                {
                    newVal = xVals[valsIndex % xVals.Length];
                }
                else
                {
                    newVal = yVals[valsIndex % yVals.Length];
                }

                composite = AppendToArray(composite, newVal);
                toggle = !toggle;
            }

            anyOffset++;
            return composite;
        }

        public virtual int[] Target(uint z)
        {
            BeforeRequest();

            bool even = totalRequests % 2 == 0;
            int[] output = { };

            for (int i = 0; i < xVals.Length; i++)
            {
                if (output.Length == z)
                {
                    break;
                }

                if (xVals[i] % 2 == (even ? 0 : 1))
                {
                    output = AppendToArray(output, xVals[i]);
                }
            }

            return output;
        }

        public int Sum(uint z)
        {
            int[] target = Target();
            BeforeRequest();

            int output = 0;
            foreach (int val in Target(z))
            {
                output += val;
            }

            return output;
        }


        protected int[] xVals = { };
        protected int[] yVals = { };

        private readonly int xMinLength;
        private readonly int yMinLength;

        private State state = State.Active;
        protected int totalRequests = 0;
        protected int failedRequests = 0;

        private int anyOffset = 0;

        public bool IsActive()
        {
            return state == State.Active;
        }

        public bool IsDeactivated()
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
            if (ArrayContains(xVals, x))
            {
                if (throwException)
                    throw new Exception($"{x} already exists in 'x' array");

                return false;
            }

            xVals = AppendToArray(xVals, x);
            return true;
        }

        protected bool AddY(int y, bool throwException = false)
        {
            if (ArrayContains(yVals, y))
            {
                if (throwException)
                    throw new Exception($"{y} already exists in 'y' array");

                return false;
            }

            yVals = AppendToArray(yVals, y);
            return true;
        }

        protected virtual void BeforeRequest(bool increment = true)
        {
            if (increment) totalRequests++;

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

        protected static int[] AppendToArray(int[] arr, int val)
        {
            int[] newArr = new int[arr.Length + 1];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i];
            }

            newArr[arr.Length] = val;
            return newArr;
        }

        private static bool ArrayContains(int[] arr, int val)
        {
            foreach (int i in arr)
            {
                if (i == val) return true;
            }

            return false;
        }
    }
}