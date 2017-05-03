using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Where.Game
{
    class Target : Engine.GameObject
    {
        public override bool Died => false;

        public Target(MapGen.Point pos)
        {
            collitor = new Box2()
            {
                Left = pos.X - 0.5f,
                Top = pos.Y - 0.5f,
                Bottom = pos.Y + 0.5f,
                Right = pos.X + 0.5f
            };
        }

        public override void OnUpdate()
        {
            var playerPos = ((GameContext)GameContext.CurrentGame.Target).Player.Position;
            if(
                playerPos.X > collitor.Left &&
                playerPos.X < collitor.Right&&
                playerPos.Y > collitor.Top &&
                playerPos.Y < collitor.Bottom
                )
            {
                //TODO:Win
                Console.WriteLine("Win!");
            }
        }

        Box2 collitor;
    }
}
