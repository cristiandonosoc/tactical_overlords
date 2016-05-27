namespace GameLogic.Utils
{
    public struct Size
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
