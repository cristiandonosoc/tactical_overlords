using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class Action
    {
        public enum ActionType
        {
            Move,
            Attack
        }

        public Entity Author { get; private set; }
        public Hexagon Target { get; private set; }
        public ActionType Type { get; private set; }

        private Map _map;

        public Action(Map map, Entity author, Hexagon target, ActionType type)
        {
            _map = map;
            Author = author;
            Target = target;
            Type = type;
        }

        internal void Execute()
        {
            switch (Type)
            {
                case ActionType.Move:
                {
                        Author.Hexagon = Target;
                        // TODO(Cristian): Collision detection
                        _map.RemoveEntityFromHexagon(Author.Hexagon, Author);
                        _map.AddEntityToHexagon(Target, Author);
                } break;
            }
        }
    }
}
