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
            public int Armor { get; set; }
            public int CurrentHealth { get; set; }
            public int MaxHealth { get; set; }
            public int CurrentMana { get; set; }
            public int MaxMana { get; set; }
        }

        #region FACTORIES

        private static int DummyEntityCount = 0;
        internal static Entity CreateDummyEntity()
        {
            // TODO(Cristian): We create random entity stats?
            EntityStats dummyStats = new EntityStats();
            string name = string.Format("DUMMY_ENTITY_{0}", DummyEntityCount);
            ++DummyEntityCount;
            Entity entity = new Entity(name, dummyStats);
            return entity;
        }

        #endregion

        // TODO(Cristian): Force that the set occurs only through the map
        // TODO(Cristian): An entity has to belong to only one cell?
        public Hexagon Hexagon { get; internal set; }

        public EntityStats Stats { get; private set; }

        // TODO(Cristian): Should name be in EntityStats?
        public string Name { get; private set; }

        #region CONSTRUCTORS 

        public Entity(string name, EntityStats stats)
        {
            Name = name;
            Stats = stats;
        }

        #endregion
    }
}
