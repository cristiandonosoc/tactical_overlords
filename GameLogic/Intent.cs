using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    internal enum Type
    {
        Move,
        Skill, // basic attack should be treated as a skill
        Interact, // trying to open a chest or do something in the map
        EndTurn // triggered at the end of a player' s turn
    }

    class Intent
    {
        #region MEMBERS

        // Who is triggering the action
        Entity _sourceEntity;

        // Action's type
        Type _type;

        // Destination hexagon
        Hexagon _destination;

        // Action's date
        DateTime _triggeredAt;

        // Action's turn
        ushort _turn;

        #endregion

        #region CONSTRUCTORS

        public Intent(Entity sourceEntity, Type type, Hexagon destination, ushort turn)
        {
            _sourceEntity = sourceEntity;
            _type = type;
            _destination = destination;
            _triggeredAt = DateTime.Now;
            _turn = turn;
        }

        #endregion
    }
}
