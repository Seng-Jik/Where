using System;
using System.Collections.Generic;
using System.Text;
using MapGen;

namespace Where.Game
{
    public class Wall : GameObjectWithCollitor
    {
        public Wall(Point pos) : base(pos)
        {
        }

        protected override void OnColliWithPlayer()
        {
            Console.WriteLine("In Wall!");
        }
    }
}
