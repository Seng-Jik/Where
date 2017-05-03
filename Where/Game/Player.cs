using System;
using OpenTK;

namespace Where.Game
{
    class Player : Engine.GameObject
    {
        public override bool Died => false;

        public Player(Renderer.IRenderer rnd,Vector2 pos)
        {
            renderer = rnd;
            angle = 0;
            position = pos;
        }

        public override void OnUpdate()
        {
            renderer.SetCamera(angle,position);
            angle += Input.Roller.XMove / 4;

            var s = Input.Runner.State;
            if (s != Input.Runner.StateType.Stop)
            {
                Vector2 delta = new Vector2(
                    (float)Math.Sin(angle*3.1415926f/180.0),
                    (float)Math.Cos(angle*3.1415926f/180.0)
                );

                delta *= -1.0f;

                position += s == Input.Runner.StateType.Go ? delta : -delta;
            }

        }

        Vector2 position;
        float angle;
        readonly Renderer.IRenderer renderer;
    }
}
