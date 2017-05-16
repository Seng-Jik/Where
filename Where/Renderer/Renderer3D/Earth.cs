using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Where.Renderer.Lower;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace Where.Renderer.Renderer3D
{
    class Earth
    {
        public Earth()
        {
            Vector3[] earth =
            {
                new Vector3(3200.0f,0,-3200.0f),
                new Vector3(0.0f,0,0.0f),
                new Vector3(3200.0f,0,0.0f),
                new Vector3(3200.0f,0,-3200.0f),
                new Vector3(0.0f,0,-3200.0f),
                new Vector3(0.0f,0,0.0f),

                /*new Vector3(1.0f,1.0f,0),
                new Vector3(-1.0f,-1.0f,0),
                new Vector3(-1.0f,1.0f,0),
                new Vector3(1.0f,1.0f,0),
                new Vector3(-1.0f,-1.0f,0),
                new Vector3(1.0f,-1.0f,0)*/
            };

            Vector2[] texCoord =
            {
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(0,1),
                new Vector2(0,0),
                new Vector2(1,0),

            };
            earthBuffer.Bind();
            earthBuffer.BufferData(3 * 6 * sizeof(float), earth, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);
            texCoordBuffer.Bind();
            texCoordBuffer.BufferData(2 * 6 * sizeof(float), texCoord, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);
        }
        public void OnDraw(Renderer3D.ObjectDrawShaderLocs locs)
        {
            earthBuffer.Bind();
            GL.VertexAttribPointer(locs.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
            texCoordBuffer.Bind();
            GL.VertexAttribPointer(locs.TexCoord, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        GLBuffer earthBuffer = new GLBuffer(BufferTarget.ArrayBuffer);
        GLBuffer texCoordBuffer = new GLBuffer(BufferTarget.ArrayBuffer);
    }
}
