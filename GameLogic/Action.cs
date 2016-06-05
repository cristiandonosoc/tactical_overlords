using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class Action
    {
        public enum Type
        {
            Move,
            Attack
        }

        public Entity Author { get; private set; }
        public Hexagon Target { get; private set; }

        public Action(Entity author, Hexagon target)
        {
            Author = author;
            Target = target;
        }
    }
}
