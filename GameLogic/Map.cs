using System.Collections.Generic;
using GameLogic.Utils;

namespace GameLogic
{
    public class Map
    {
        #region INTERFACE

        public Hexagon[] Grid { get { return _grid; } }
        public ushort Turn { get { return _turn; } }

        public Hexagon GetHexagon(uint x, uint y)
        {
            if ((x < 0) || (x >= _mapSize.Width) ||
                (y < 0) || (y >= _mapSize.Height))
            {
                return null;
            }

            return _grid[(int)(_mapSize.Width * y + x)];
        }

        #endregion

        #region MEMBERS

        // For now, keep it simple
        // TODO(Cristian): Do we need another data structure? 
        // With List<Hexagon> it's awkward because you have to do
        // _grid.Append(null) for an empty hexagon. I feel you don't win anything
        // vs an array. If we need dynamic we would need a hash table
        // (See Hash Table vs Dictionary<uint "key", Hexagon>)
        Hexagon[] _grid;

        // Players parties (with >2 players? wow, such game, very fun)
        List<Party> _partyList;

        // Map size
        Size _mapSize;

        // This should be an object, with different states
        ushort _turn;

        #endregion

        #region CONSTRUCTORS

        public Map(uint width, uint height, uint partyNumber)
        {
            Size size = new Size((ushort)width, (ushort)height);
            Initialize(size, (ushort)partyNumber);
        }

        public Map(Size mapSize, ushort partyNumber)
        {
            Initialize(mapSize, partyNumber);
        }

        // NOTE(Cristian): This has to a separate because you cannot call a
        // constructor from within another constructor in C# 
        // and I wanted overloaded constructors
        private void Initialize(Size mapSize, uint partyNumber)
        {
            _partyList = new List<Party>();
            _mapSize = mapSize;
            _turn = 0;

            // Grid initialization
            _grid = new Hexagon[_mapSize.Width * _mapSize.Height];

            // For now we initiallize all the hexagons
            for (ushort y = 0; y < _mapSize.Height; ++y)
            {
                for (ushort x = 0; x < _mapSize.Width; ++x)
                {
                    _grid[_mapSize.Width * y + x] = new Hexagon();
                }
            }
        }

        #endregion
    }
}
