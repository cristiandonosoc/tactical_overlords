using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic.Utils
{
    /**
     * This is so the whole GameLogic uses only one random instance
     */
    static class RandomSingleton
    {
        #region SINGLETON

        private static Random _randomInstance;

        internal static Random GenerateInstance(int seed = 0)
        {
            if (seed == 0)
            {
                // Use system defined constant
                _randomInstance = new Random();
            }
            else
            {
                _randomInstance = new Random(seed);
            }
            return _randomInstance;
        }

        internal static Random GetInstance()
        {
            if (_randomInstance == null)
            {
                GenerateInstance();
            }

            return _randomInstance;
        }

        #endregion
    }
}
