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
                playerPos.X > collitor.Left - 0.15f &&
                playerPos.X < collitor.Right + 0.15f &&
                playerPos.Y > collitor.Top - 0.15f &&
                playerPos.Y < collitor.Bottom + 0.15f
                )
            {
                OnColliWithPlayer();
            }
        }

        protected abstract void OnColliWithPlayer();

        private readonly Box2 collitor;

        protected Box2 Collitor { get => collitor; }
    }
}