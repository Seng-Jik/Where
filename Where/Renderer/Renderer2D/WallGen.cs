using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace Where.Renderer.Renderer2D
{
    public static class WallGen
    {
        public static SceneBuffer CreateWallBuffer(List<MapGen.Point> wallPoints)
        {
            var sceneBuffer = new SceneBuffer();

            List<OpenTK.Vector2> vertexBuffer = new List<Vector2>();
            List<short> index = new List<short>();
            short nowIndex = 0;
            foreach(var wall in wallPoints)
            {
                vertexBuffer.Add(new Vector2(wall.X - 0.5f, wall.Y - 0.5f));
                vertexBuffer.Add(new Vector2(wall.X + 0.5f, wall.Y - 0.5f));
                vertexBuffer.Add(new Vector2(wall.X + 0.5f, wall.Y + 0.5f));
                vertexBuffer.Add(new Vector2(wall.X - 0.5f, wall.Y + 0.5f));

                index.Add(nowIndex);
                index.Add((short)(nowIndex + 1));
                index.Add((short)(nowIndex + 2));
                index.Add(nowIndex);
                index.Add((short)(nowIndex + 3));
                index.Add((short)(nowIndex + 2));
                nowIndex += 4;
            }

            sceneBuffer.IndexBuffer = new Lower.GLBuffer(BufferTarget.ElementArrayBuffer);
            sceneBuffer.IndexBuffer.BufferData<short>(index.Count * sizeof(short), index.ToArray(), BufferUsageHint.StaticDraw);
            sceneBuffer.VertexBuffer = new Lower.GLBuffer(BufferTarget.ArrayBuffer);
            sceneBuffer.VertexBuffer.BufferData<Vector2>(vertexBuffer.Count * 2 * sizeof(float), vertexBuffer.ToArray(), BufferUsageHint.StaticDraw);
            sceneBuffer.VerticleSize = index.Count;

            return sceneBuffer;
        }
    }
}
