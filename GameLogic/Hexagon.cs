using GameLogic.Utils;
namespace GameLogic
{
    public class Hexagon
    {
        #region HELPER

        // TODO(Cristian): Should this neighbours be stored directly in each
        // hexagon? The direct lookup seems ok... (not even a hash table lookup)
        /// <summary>
        /// Gets the neighbours of a hexagon
        /// </summary>
        /// <param name="context">The map where the hexagon resides</param>
        /// <param name="hexagon">The hexagon to which to get the neighbours</param>
        /// <param name="resultBuffer">
        /// Stores the result in a pre-allocated buffer
        /// The result is as follows: [SEE DEFINITION OF METHOD]
        ///             ___
        ///         ___/   \___          
        ///        /   \_0_/   \     
        ///        \_5_/   \_1_/                 
        ///        /   \___/   \     
        ///        \_4_/   \_2_/                 
        ///            \_3_/
        /// </param>
        public static void GetHexagonNeighbours(Map context, Hexagon hexagon,
                                                 ref Hexagon[] resultBuffer)
        {
            resultBuffer[0] = context.GetHexagon(hexagon.X    , hexagon.Y + 1);
            resultBuffer[1] = context.GetHexagon(hexagon.X + 1, hexagon.Y    );
            resultBuffer[2] = context.GetHexagon(hexagon.X + 1, hexagon.Y - 1);
            resultBuffer[3] = context.GetHexagon(hexagon.X    , hexagon.Y - 1);
            resultBuffer[4] = context.GetHexagon(hexagon.X - 1, hexagon.Y + 0);
            resultBuffer[5] = context.GetHexagon(hexagon.X - 1, hexagon.Y + 1);
        }
        
        public static int GenerateIndex(Map context, Hexagon hexagon)
        {
            int result = context.Size.Width * hexagon.Y + hexagon.X;
            return result;
        }

        public static uint GenerateXYKey(ushort x, ushort y)
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



        #endregion

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

        // For easier debugging/watching in Visual Studio
        public override string ToString()
        {
            return string.Format("{0}_{1}", this.X, this.Y);
        }
    }
}
