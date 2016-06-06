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

        public Action(Entity author, Hexagon target, ActionType type)
        {
            Author = author;
            Target = target;
            Type = type;
        }
    }
}
