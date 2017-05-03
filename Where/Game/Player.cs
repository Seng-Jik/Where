using System;
using OpenTK;

namespace Where.Game
{
    class Player : Engine.GameObject
    {
        public override bool Died => false;

        public Player(Renderer.IRenderer rnd)
        {
            renderer = rnd;
            angle = 0;
            position = new Vector2(0, 0);
        }

        public override void OnUpdate()
        {
            renderer.SetCamera(angle,position);
            angle += Input.Roller.XMove / 4;

            var s = Input.Runner.State;
            if (s != Input.Runner.StateType.Stop)
            {
                Vector2 delta = new Vector2(
                    (float)Math.Sin(angle),
                    (float)Math.Cos(angle)
                );

                delta *= 0.025f;

                position += s == Input.Runner.StateType.Go ? delta : -delta;
            }

        }

        Vector2 position;
        float angle;
        readonly Renderer.IRenderer renderer;
    }
}
