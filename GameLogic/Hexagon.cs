using GameLogic.Utils;
namespace GameLogic
{
    public class Hexagon
    {
        #region INTERFACE

        public Point Size { get; private set; }
        public ushort X { get { return Size.Width; } }
        public ushort Y { get { return Size.Height; } }

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
        }
    }
}
