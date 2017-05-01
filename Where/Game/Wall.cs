using System;
using System.Collections.Generic;
using System.Text;

namespace Where.Game
{
    public class Wall : Engine.GameObject
    {
        public override bool Died => false;

        public override void OnUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
