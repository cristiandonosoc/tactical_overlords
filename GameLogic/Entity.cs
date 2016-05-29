using GameLogic.Utils;
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

            #region FACTORIES
            internal static EntityStats CreateDummyStats()
            {
                EntityStats stats = new EntityStats();

                Random random = RandomSingleton.GetInstance();
                // We create random values
                stats.Armor = random.Next(0, 5);
                stats.MaxHealth = random.Next(5, 25) * 50;
                stats.CurrentHealth = random.Next(10, stats.MaxHealth);
                stats.MaxMana = random.Next(0, 10) * 10;
                stats.CurrentMana = random.Next(0, stats.MaxMana);
                stats.MovementRange = random.Next(1, 4);

                return stats;
            }

            #endregion

            // Basic char stats
            public int Armor { get; internal set; }
            public int CurrentHealth { get; internal set; }
            public int MaxHealth { get; internal set; }
            public int CurrentMana { get; internal set; }
            public int MaxMana { get; internal set; }

            public int MovementRange { get; internal set; }
        }

        #region FACTORIES

        private static int DummyEntityCount = 0;
        internal static Entity CreateDummyEntity()
        {
            // TODO(Cristian): We create random entity stats?

            string name = string.Format("DUMMY_ENTITY_{0}", DummyEntityCount);
            ++DummyEntityCount;
            Entity entity = new Entity(name,
                                       EntityStats.CreateDummyStats());
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
