using MapGen;
using System.Windows.Forms;

namespace Where.Game
{
    internal class Target : GameObjectWithCollitor
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