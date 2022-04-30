namespace DataExtractors
{
    public enum State
    {
        Active,
        Inactive,
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
                    state = State.Inactive;
                }
            }

            if (HasInvalidLengths())
            {
                state = State.Inactive;
            }
        }

        public virtual int[] Any()
        {
            BeforeRequest();

            return AnyHelper();
        }

        public virtual int[] Target(uint z)
        {
            BeforeRequest();

            return TargetHelper(z);
        }

        public int Sum(uint z)
        {
            BeforeRequest();

            int[] target = TargetHelper(z);

            int output = 0;
            foreach (int val in target)
            {
                output += val;
            }

            return output;
        }


        protected int[] xVals = { };
        protected int[] yVals = { };

        private readonly int xMinLength;
        private readonly int yMinLength;

        protected State state = State.Active;
        protected int totalRequests = 0;
        protected int failedRequests = 0;

        private int anyOffset = 0;

        public State GetState()
        {
            return state;
        }

        public bool IsActive()
        {
            return state == State.Active;
        }

        public bool IsInactive()
        {
            return state == State.Inactive;
        }

        protected void MarkInactive()
        {
            failedRequests++;
            if (state != State.Deactivated)
            {
                state = State.Inactive;
            }

            throw new Exception("Invalid request");
        }

        private bool AddX(int x, bool throwException = false)
        {
            if (ArrayContains(xVals, x))
            {
                if (throwException)
                    throw new Exception($"{x} already exists in 'x' array");

                return false;
            }

            AppendToArray(ref xVals, x);
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

            AppendToArray(ref yVals, y);
            return true;
        }

        protected int[] AnyHelper()
        {
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

                AppendToArray(ref composite, newVal);
                toggle = !toggle;
            }

            anyOffset++;
            return composite;
        }

        private int[] TargetHelper(uint z)
        {
            bool even = z % 2 == 0;
            int[] output = { };

            for (int i = 0; i < xVals.Length; i++)
            {
                if (output.Length == z)
                {
                    break;
                }

                if (xVals[i] % 2 == (even ? 0 : 1))
                {
                    AppendToArray(ref output, xVals[i]);
                }
            }

            return output;
        }

        protected virtual void BeforeRequest()
        {
            totalRequests++;

            if (IsInactive())
            {
                failedRequests++;
                throw new Exception("Invalid request");
            }
        }

        private bool HasInvalidLengths()
        {
            return xVals.Length < xMinLength || yVals.Length < yMinLength;
        }

        protected static void AppendToArray(ref int[] arr, int val)
        {
            int[] newArr = new int[arr.Length + 1];
            for (int i = 0; i < arr.Length; i++)
            {
                newArr[i] = arr[i];
            }

            newArr[arr.Length] = val;
            arr = newArr;
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