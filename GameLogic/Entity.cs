using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{

    public class Entity
    {

        #region FACTORIES

        internal static Entity CreateDummyEntity()
        {
            Entity entity = new Entity();
            return entity;
        }

        #endregion

        // TODO(Cristian): Force that the set occurs only through the map
        // TODO(Cristian): An entity has to belong to only one cell?
        public Hexagon Hexagon { get; internal set; }
    }
}
