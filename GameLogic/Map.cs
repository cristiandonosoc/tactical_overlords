using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.Utils;

namespace GameLogic
{
    public class Map
    {
        #region INTERFACE

        public Hexagon[] Grid { get { return _grid; } }
        public ushort Turn { get { return _turn; } }

        #endregion

        // For now, keep it simple
        Hexagon[] _grid;

        // Players parties (with >2 players? wow, such game)
        List<Party> _partyList;

        // Map size
        Size _mapSize;

        // This should be an object, with different states
        ushort _turn;

        public Map(Size mapSize, ushort partyNumber)
        {
            _grid = new Hexagon[mapSize.Width + mapSize.Height];
            _partyList = new List<Party>();
            _mapSize = mapSize;
            _turn = 0;
        }
    }
}
