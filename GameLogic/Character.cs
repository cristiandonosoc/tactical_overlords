using System;
using GameLogic.Utils;

namespace GameLogic
{
    class Character
    {
        #region INTERFACE

        public String Name { get { return _name; } }

        public uint Armor { get { return _armor; } }
        public uint CurrentHealth { get { return _currentHealth; } }
        public uint MaxHealth { get { return _maxHealth; } }
        public uint CurrentMana { get { return _currentMana; } }
        public uint MaxMana { get { return _maxMana; } }

        public Size Position { get { return _position; } }

        #endregion

        String _name;

        // Basic char stats
        uint _armor;
        uint _currentHealth;
        uint _maxHealth;
        uint _currentMana;
        uint _maxMana;

        // Action values
        ushort _movementCount;

        // Position on the map, should it be a Hex from the grid??
        Size _position;

        public Character(String name, uint armor, uint maxHealth, uint maxMana, 
                         ushort movementCount, Size position)
        {
            _name = name;
            _armor = armor;
            _currentHealth = _maxHealth = maxHealth;
            _currentMana = _maxMana = maxMana;
            _movementCount = movementCount;
            _position = position;
        }
    }
}
