using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace Where.Renderer
{
    public interface IRenderer
    {
        void OnDraw();

        void SetWallBuffer(List<MapGen.Point> wallPoints);
        void SetCamera(float angle, Vector2 pos);
    }
}
