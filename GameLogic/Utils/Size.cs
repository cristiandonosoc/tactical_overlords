﻿namespace GameLogic.Utils
{
    public struct Size
    {
        #region INTERFACE

        public ushort Width
        {
            get { return _x; }
            set { _x = value; }
        }
        public ushort Height 
        {
            get { return _y; }
            set { _y = value; }
        }
        public ushort X 
        {
            get { return _x; }
            set { _x = value; }
        }
        public ushort Y 
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        private ushort _x;
        private ushort _y;

        public Size(ushort width, ushort height)
        {
            _x = width;
            _y = height;
        }
    }
}
