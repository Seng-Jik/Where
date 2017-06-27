using OpenTK;
using System;

namespace Where.Game
{
    public class Player : Engine.GameObject
    {
        public override bool Died => false;

        public Player(Renderer.IRenderer rnd, Vector2 pos)
        {
            renderer = rnd;
            angle = 0;
            keyMovespeed = 0;
            LastPosition = Position = pos;
        }

        public override void OnUpdate()
        {
            LastPosition = Position;
            renderer.SetCamera(angle, pov, Position);
            angle += Input.Roller.XDelta / 4;
            pov += Input.Roller.YDelta / 4;

            if (pov > 45) pov = 45;
            else if (pov < -45) pov = -45;

            var s = Input.Runner.State;
            var r = Input.Runner.IsKeyDown;
            var w = Input.Runner.MouseWheeled;
            Vector2 endDelta = new Vector2(0, 0);
            Vector2 tmpDelta = new Vector2(0, 0);
            if (r)
            {
                keyMovespeed = 0.2f;
                if (Math.Abs(keyMovespeed) > 0.0f)
                {
                    keyMovespeed *= 0.7f;
                    if (Math.Abs(keyMovespeed) < 0.05f)
                        keyMovespeed = 0.0f;
                }
                Vector2 delta = new Vector2(
                    (float)Math.Sin((angle + Where.Input.Runner.AngleFix) * 3.1415926f / 180.0f),
                    (float)Math.Cos((angle + Where.Input.Runner.AngleFix) * 3.1415926f / 180.0f)
                );
                delta *= -1.0f * keyMovespeed;
                endDelta = delta;
                tmpDelta = delta;
            }
            if (w) {
                if (s != Input.Runner.StateType.Stop)
                {
                    mouseMoveSpeed = s == Input.Runner.StateType.Go ? 0.2f : -0.2f;
                }
            }
            if (Math.Abs(mouseMoveSpeed) > 0.0f)
            {
                mouseMoveSpeed *= 0.7f;
                if (Math.Abs(mouseMoveSpeed) < 0.05f)
                {
                    mouseMoveSpeed = 0.0f;
                    Input.Runner.MouseWheeled = false;
                }
                Vector2 delta = new Vector2(
                     (float)Math.Sin((angle) * 3.1415926f / 180.0f),
                     (float)Math.Cos((angle) * 3.1415926f / 180.0f)
                 );
                delta *= -1.0f * mouseMoveSpeed;

                tmpDelta += delta;

                if (Math.Sqrt(tmpDelta.X * tmpDelta.X + tmpDelta.Y * tmpDelta.Y) <= 1) endDelta = tmpDelta;
            }
            Position += endDelta;
        }

        public Vector2 Position { get; set; }
        public Vector2 LastPosition { get; private set; }
        private float angle, keyMovespeed,mouseMoveSpeed, pov;
        private readonly Renderer.IRenderer renderer;
    }
}