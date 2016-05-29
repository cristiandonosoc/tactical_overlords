using GameLogic.Utils;
namespace GameLogic
{
    public class Hexagon
    {
        #region INTERFACE

        public Point Size { get; private set; }
        public ushort X { get { return Size.Width; } }
        public ushort Y { get { return Size.Height; } }
        public uint Key { get; private set; }

        // TODO(Cristian): This is probably not the way we want to model this
        private Entity _entity;
        public Entity Entity
        {
            get { return _entity; }
            set
            {
                if (!Map.EntityAddThroughMap)
                {
                    string message = "Entity added to hexagon without going through the map";
                    throw new System.InvalidOperationException(message);
                }

                _entity = value;
            }
        }

        #endregion

        internal Hexagon(ushort x, ushort y)
        {
            Size = new Point(x, y);
            Key = GenerateXYKey(x, y);
        }

        static uint GenerateXYKey(ushort x, ushort y)
        {
            // This unchecked is so that C# doesn't try to check bounds
            // and interpret the bytes directly
            // TODO(Cristian): Do some tests to check how this actually behaves
            uint key;
            unchecked
            {
                key = (uint)((x << 16) | y);
            }
            return key;
        }

        public override string ToString()
        {
            return string.Format("{0}_{1}", this.X, this.Y);
        }
    }
}
