using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Utils
{
    public class Size
    {
        #region INTERFACE

        public ushort Width { get { return _width; } }
        public ushort Height { get { return _height; } }

        #endregion

        ushort _width;
        ushort _height;

        public Size(ushort width, ushort height)
        {
            _width = width;
            _height = height;
        }
    }
}
