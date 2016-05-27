using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class Map
    {
        #region INTERFACE

        public Hexagon[] Grid { get { return _grid; } }

        #endregion

        // For now, keep it simple
        Hexagon[] _grid;




    }
}
