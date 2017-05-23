using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Where.Renderer.Lower;
using OpenTK.Graphics.ES20;

namespace Where.Renderer.Renderer3D
{
    class SkyBox
    {
        public SkyBox()
        {
            time = 0;
            Vector3[] vertData =
            {
                new Vector3(-500.0f,200.0f,-500.0f),
                new Vector3(-500.0f,200.0f,500.0f),
                new Vector3(500.0f,200.0f,500.0f),
                new Vector3(500.0f,200.0f,-500.0f),
            };

            verticles.Bind();
            verticles.BufferData(3 * sizeof(float) * 4, vertData, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);

            Vector2[] texCoordData =
            {
                new Vector2(1,1),
                new Vector2(0,1),
                new Vector2(0,0),
                new Vector2(1,0)
            };
            texCoord.Bind();
            texCoord.BufferData(2 * sizeof(float) * 4, texCoordData, BufferUsageHint.StaticDraw);

            ushort[] indData =
            {
                2,1,0,3,2,0
            };
            indices.Bind();
            indices.BufferData(6 * sizeof(ushort), indData, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);

            skyTopShader = new GLShader("3D_SkyBoxVertex", "3D_SkyBox");
            skyTopShader.Use();
            topLocs.Vertex = skyTopShader.GetAttributionLocation("Vertex");
            skyTopShader.EnableAttribute(topLocs.Vertex);
            topLocs.TexCoord = skyTopShader.GetAttributionLocation("TexCoordInp");
            skyTopShader.EnableAttribute(topLocs.TexCoord);
            topLocs.Camera = skyTopShader.GetUniformLocation("Camera");
            topLocs.Time = skyTopShader.GetUniformLocation("Time");
            skyTopShader.SetUniform("Perlin", 3);

            GL.UseProgram(0);
        }

        public void OnDraw()
        {
            skyTopShader.Use();
            skyTopShader.SetUniform(topLocs.Time, time);
            verticles.Bind();
            GL.VertexAttribPointer(topLocs.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
            texCoord.Bind();
            GL.VertexAttribPointer(topLocs.TexCoord, 2, VertexAttribPointerType.Float, false, 0, 0);
            indices.Bind();
            GL.DrawElements(BeginMode.Triangles, 6,DrawElementsType.UnsignedShort, 0);
            GL.UseProgram(0);
        }

        public void SetPos(Vector2 pos,Renderer3D rnd)
        {
            time++;
            var modelview = Matrix4.CreateTranslation(new Vector3(21.0F * pos.X, 0, -21.0F * pos.Y));
            var transform = modelview * rnd.Camera * rnd.Projection;

            skyTopShader.Use();
            skyTopShader.SetUniform(topLocs.Camera, ref transform);
            GL.UseProgram(0);
        }

        GLBuffer verticles = new GLBuffer(OpenTK.Graphics.ES20.BufferTarget.ArrayBuffer);
        GLBuffer indices = new GLBuffer(OpenTK.Graphics.ES20.BufferTarget.ElementArrayBuffer);
        GLBuffer texCoord = new GLBuffer(OpenTK.Graphics.ES20.BufferTarget.ArrayBuffer);

        GLShader skyTopShader;
        float time;

        struct SkyTopShaderLocs
        {
            public int
                Camera,
                Vertex,
                TexCoord,
                Time;
        }

        SkyTopShaderLocs topLocs;
    }
}
