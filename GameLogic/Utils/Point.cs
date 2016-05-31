namespace GameLogic.Utils
{
    public struct Point
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

        public Point(ushort x, ushort y)
        {
            _x = x;
            _y = y;
        }
    }
}
