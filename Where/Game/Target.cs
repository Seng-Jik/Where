using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapGen;
using OpenTK;

namespace Where.Game
{
    class Target : GameObjectWithCollitor
    {
        public Target(Point pos) : base(pos)
        {
        }

        protected override void OnColliWithPlayer()
        {
            Console.WriteLine("Win!");
        }
    }
}
