using System;
using OpenTK;

namespace Where.Game
{
    public class Player : Engine.GameObject
    {
        public override bool Died => false;

        public Player(Renderer.IRenderer rnd,Vector2 pos)
        {
            renderer = rnd;
            angle = 0;
            speed = 0;
            LastPosition = Position = pos;
        }

        public override void OnUpdate()
        {
            LastPosition = Position;
            renderer.SetCamera(angle,Position);
            angle += Input.Roller.XMove / 4;

            var s = Input.Runner.State;

            if (s != Input.Runner.StateType.Stop)
            {
                speed = s == Input.Runner.StateType.Go ? 0.2f : -0.2f;
            }

            if(Math.Abs(speed) > 0.0f)
            {
                speed *= 0.7f;
                if (Math.Abs(speed) < 0.05f)
                    speed = 0.0f;
            }

            Vector2 delta = new Vector2(
                (float)Math.Sin(angle * 3.1415926f / 180.0f),
                (float)Math.Cos(angle * 3.1415926f / 180.0f)
            );

            delta *= -1.0f * speed;

            Position += delta;
        }

        public Vector2 Position { get; set; }
        public Vector2 LastPosition { get; private set; }
        float angle,speed;
        readonly Renderer.IRenderer renderer;
    }
}
