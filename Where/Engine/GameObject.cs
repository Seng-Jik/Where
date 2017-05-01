using System;
using System.Collections.Generic;
using System.Text;

namespace Where.Engine
{
    public abstract class GameObject
    {
        public abstract void OnUpdate();
        public abstract bool Died { get; }
    }
}
