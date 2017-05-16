using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapGen;
using OpenTK;
using System.Windows.Forms;

namespace Where.Game
{
    class Target : GameObjectWithCollitor
    {
        public Target(Point pos) : base(pos)
        {
        }

        protected override void OnColliWithPlayer()
        {
            MessageBox.Show("Win!");
            Engine.Engine.Window.Close();
        }
    }
}
