using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Utils
{
    class Position
    {
        #region INTERFACE

        public short X { get; set; }
        public short Y { get; set; }

        #endregion

        public Position(short x, short y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
