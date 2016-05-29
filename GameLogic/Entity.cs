using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{

    public class Entity
    {
        public class EntityStats
        {
            // Basic char stats
            public int Armor;
            public int CurrentHealth;
            public int MaxHealth;
            public int CurrentMana;
            public int MaxMana;
        }

        #region FACTORIES

        internal static Entity CreateDummyEntity()
        {
            // TODO(Cristian): We create random entity stats?
            EntityStats dummyStats = new EntityStats();
            Entity entity = new Entity(dummyStats);
            return entity;
        }

        #endregion

        // TODO(Cristian): Force that the set occurs only through the map
        // TODO(Cristian): An entity has to belong to only one cell?
        public Hexagon Hexagon { get; internal set; }

        public EntityStats Stats { get; internal set; }

        #region CONSTRUCTORS 

        public Entity(EntityStats stats)
        {
            Stats = stats;
        }

        #endregion
    }
}
