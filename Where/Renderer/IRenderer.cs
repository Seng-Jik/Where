using OpenTK;
using System.Collections.Generic;

namespace Where.Renderer
{
    public interface IRenderer
    {
        void OnDraw();

        void SetWallBuffer(List<MapGen.Point> wallPoints, MapGen.Point targetPoint);

        void SetCamera(float angle, float pov, Vector2 pos);
    }
}