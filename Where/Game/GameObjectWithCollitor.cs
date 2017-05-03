using System;
using OpenTK;

namespace Where.Game
{
    public abstract class GameObjectWithCollitor : Engine.GameObject
    {
        public override bool Died => false;

        public GameObjectWithCollitor(MapGen.Point pos)
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
            if (
                playerPos.X > collitor.Left &&
                playerPos.X < collitor.Right &&
                playerPos.Y > collitor.Top &&
                playerPos.Y < collitor.Bottom
                )
            {
                OnColliWithPlayer();
            }
        }

        protected abstract void OnColliWithPlayer();

        Box2 collitor;
    }
}
