using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public enum Type
    {
        Move,
        Skill, // basic attack should be treated as a skill
        Interact // trying to open a chest or do something in the map
    }

    class Action
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

        #endregion

        #region CONSTRUCTORS

        public Action(Entity sourceEntity, Type type, Hexagon destination)
        {
            _sourceEntity = sourceEntity;
            _type = type;
            _destination = destination;
            _triggeredAt = DateTime.Now;
        }

        #endregion

    }
}
