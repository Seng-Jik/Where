using System.Collections.Generic;

namespace Where.Engine
{
    public class GameObjectList : GameObject
    {
        public GameObjectList()
        {
            Objects = new List<GameObject>();
        }

        public override bool Died => false;

        public List<GameObject> Objects { get; private set; }

        public override void OnUpdate()
        {
            foreach (var i in Objects)
                i.OnUpdate();
            Objects.RemoveAll((obj) => obj.Died);
        }
    }
}