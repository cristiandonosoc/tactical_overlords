﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Utils
{
    public class Size
    {
        #region INTERFACE

        public ushort Width { get; set; }
        public ushort Height { get; set; }

        #endregion

        public Size(ushort width, ushort height)
        {
            this.Width = width;
            this.Height = height;
        }
    }
}
