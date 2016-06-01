using System.Collections.Generic;
using GameLogic.Utils;

namespace GameLogic
{
    public class Map
    {
        #region UNREASONABLE CODE PATTERNS
        // This static bool is used to ensure all entities are added
        // through the map. The setter of entity will check that this boolean
        // is set. If not, an exception will be thrown.
        // Just throwing ideas around.
        internal static bool EntityAddThroughMap = false;

        #endregion


        #region INTERFACE

        public Hexagon[] Grid { get { return _grid; } }
        public ushort Turn { get { return _turn; } }

        public Hexagon GetHexagon(int x, int y)
        {
            if ((x < 0) || (x >= _mapSize.Width) ||
                (y < 0) || (y >= _mapSize.Height))
            {
                return null;
            }

            return _grid[_mapSize.Width * y + x];
        }

        public List<Entity> GetEntities()
        {
            var values = _characterCoordDictionary.Values;
            // TODO(Cristian): This is crazy slow
            List<Entity> entityList = new List<Entity>(values.Count);
            foreach (Entity entity in values)
            {
                entityList.Add(entity);
            }

            return entityList;
        }

        public Entity GetEntity(Hexagon hexagon)
        {
            if (!_characterCoordDictionary.ContainsKey(hexagon.Key)) { return null; }
            Entity result = _characterCoordDictionary[hexagon.Key];
            return result;
        }

        #endregion

        // All entity adding *should* go through here
        private bool AddEntityToHexagon(int x, int y, Entity entity)
        {
            Hexagon hex = GetHexagon(x, y);
            if ((hex == null) || (hex.Entity != null)) { return false; }

            bool result = AddEntityToHexagon(hex, entity);
            return result;
        }

        private bool AddEntityToHexagon(Hexagon hexagon, Entity entity)
        {
            // This is a valid adding path
            EntityAddThroughMap = true;
            // This is an extra check, though this should not happen!
            if (_characterCoordDictionary.ContainsKey(hexagon.Key))
            {
                // TODO(Cristian): Diagnose this!
                return false;
            }
            _characterCoordDictionary.Add(hexagon.Key, entity);
            hexagon.Entity = entity;
            entity.Hexagon = hexagon;
            EntityAddThroughMap = false;
            return true;
        }

        public Point Size { get { return _mapSize; } }

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

        // TODO(Cristian): Obtaining characters like this probably will change
        Dictionary<uint, Entity> _characterCoordDictionary;

        // Map size
        Point _mapSize;

        // This should be an object, with different states
        ushort _turn;

        #endregion

        #region CONSTRUCTORS

        public Map(uint width, uint height, uint partyNumber)
        {
            Point size = new Point((ushort)width, (ushort)height);
            Initialize(size, (ushort)partyNumber);
        }

        public Map(Point mapSize, ushort partyNumber)
        {
            Initialize(mapSize, partyNumber);
        }

        // NOTE(Cristian): This has to a separate because you cannot call a
        // constructor from within another constructor in C# 
        // and I wanted overloaded constructors
        private void Initialize(Point mapSize, uint partyNumber)
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
                    _grid[_mapSize.Width * y + x] = new Hexagon(x, y);
                }
            }

            // We initialize the lookup table
            _characterCoordDictionary = new Dictionary<uint, Entity>();

            // We add a dummy character
            AddEntityToHexagon(1, 1, Entity.CreateDummyEntity());
            AddEntityToHexagon(2, 5, Entity.CreateDummyEntity());
        }

        #endregion
    }
}
