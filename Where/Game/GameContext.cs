using System;
using System.Collections.Generic;
using System.Text;

namespace Where.Game
{
    public class GameContext:Engine.GameObjectList
    {
        public GameContext(int width,int height)
        {
            var map = MapGen.MapGen.NewMap(width,height);
            for(int y = 0;y < map.Height; ++y)
            {
                for (int x = 0; x < map.Width; ++x)
                {

                }
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

    }
}
