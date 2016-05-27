using System.Collections.Generic;
using GameLogic.Utils;

namespace GameLogic
{
    public class Map
    {
        #region INTERFACE

        public List<Hexagon> Grid { get { return _grid; } }
        public ushort Turn { get { return _turn; } }

        #endregion

        // For now, keep it simple
        List<Hexagon> _grid;

        // Players parties (with >2 players? wow, such game, very fun)
        List<Party> _partyList;

        // Map size
        Size _mapSize;

        // This should be an object, with different states
        ushort _turn;

        // NOTE(Cristian): This has to a separate because you cannot call a
        // constructor from within another constructor and I wanted 
        // overloaded constructors
        private void Initialize(Size mapSize, uint partyNumber)
        {
            _partyList = new List<Party>();
            _mapSize = mapSize;
            _turn = 0;

            // Grid initialization
            _grid = new List<Hexagon>(_mapSize.Width + _mapSize.Height);

            // For now we initiallize all the hexagons
            for (ushort y = 0; y < _mapSize.Height; ++y)
            {
                for (ushort x = 0; x < _mapSize.Width; ++x)
                {
                    _grid[_mapSize.Width * y + x] = new Hexagon();
                }
            }
        }

        public Map(uint width, uint height, uint partyNumber)
        {
            Size size = new Size((ushort)width, (ushort)height);
            Initialize(size, (ushort)partyNumber);
        }

        public Map(Size mapSize, ushort partyNumber)
        {
            Initialize(mapSize, partyNumber);
        }
    }
}
