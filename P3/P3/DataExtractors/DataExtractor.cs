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
                throw new InvalidDataException("x can't be an empty array");
            }

            xMinLength = (int)Math.Ceiling(x.Length / 2.0);
            yMinLength = x.First() % 10 + 1;

            foreach (int currX in x)
            {
                AddX(currX);
            }

            for (int i = 0; i < yMinLength; i++)
            {
                int xMappedIndex = i % x.Length;
                AddY(_xVals[xMappedIndex] * 2);
            }
        }

        public virtual int[] any()
        {
            bool toggle = true;
            int maxLen = Math.Max(_xVals.Length, _yVals.Length);
            int minLen = Math.Min(_xVals.Length, _yVals.Length);

            int[] composite = Array.Empty<int>();

            // Alternate adding elements from both array
            for (int i = 0; i < minLen; i++)
            {
                if (toggle)
                {
                    composite[i] = _xVals[(i + anyOffset) % _xVals.Length];
                }
                else
                {
                    composite[i] = _yVals[i];
                }
            }

            // Fill the remaining elements from the longer array
            for (int i = minLen; i < maxLen; i++)
            {
                composite[i] = _xVals.Length == maxLen ? _xVals[i] : _yVals[i];
            }

            anyOffset++;
            return composite;
        }

        public virtual int[] target(uint z)
        {
        }

        public int sum(uint z)
        {
        }


        private int[] _xVals;
        private int[] _yVals = Array.Empty<int>();
        private int xMinLength;
        private int yMinLength;

        protected State state = State.Active;
        private int anyOffset = 0;

        protected int totalRequests = 0;
        protected int failedRequests = 0;

        protected virtual int[] GetXs()
        {
            return _xVals;
        }

        protected virtual int[] GetYs()
        {
            return _yVals;
        }

        protected bool AddX(int x)
        {
            if (_xVals.Contains(x)) return false;

            _xVals = _xVals.Append(x).ToArray();
            return true;
        }

        protected bool AddY(int y)
        {
            if (_yVals.Contains(y)) return false;

            _yVals = _yVals.Append(y).ToArray();
            return true;
        }

        protected virtual void beforeRequest()
        {
            totalRequests++;

        }
    }
}