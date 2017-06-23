using OpenTK;
using OpenTK.Graphics.ES20;
using Where.Renderer.Lower;

namespace Where.Renderer.Renderer3D
{
    internal class SkyBox
    {
        public SkyBox()
        {
            time = 0;
            Vector3[] vertData =
            {
                new Vector3(-500.0f,250.0f,-500.0f),
                new Vector3(-500.0f,250.0f,500.0f),
                new Vector3(500.0f,250.0f,500.0f),
                new Vector3(500.0f,250.0f,-500.0f),

                new Vector3(-500.0f,-250f,-500.0f),
                new Vector3(-500.0f,-250f,500.0f),
                new Vector3(500.0f,-250f,500.0f),
                new Vector3(500.0f,-250f,-500.0f),
            };

            verticles.Bind();
            verticles.BufferData(3 * sizeof(float) * 8, vertData, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);

            Vector2[] texCoordData =
            {
                new Vector2(1,1),
                new Vector2(0,1),
                new Vector2(0,0),
                new Vector2(1,0),

                new Vector2(1,0),
                new Vector2(1,1),
                new Vector2(0,1),
                new Vector2(0,0)
            };
            texCoord.Bind();
            texCoord.BufferData(2 * sizeof(float) * 8, texCoordData, BufferUsageHint.StaticDraw);

            ushort[] indData =
            {
                2,1,0,3,2,0,
                3,6,2,3,7,6,
                1,4,0,1,5,4,
                2,5,1,2,6,5,
                7,3,0,4,7,0
            };
            indices.Bind();
            indices.BufferData(6 * sizeof(ushort) * 5, indData, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);

            skyTopShader = new GLShader("3D_SkyBoxVertex", "3D_SkyBox");
            skyTopShader.Use();
            topLocs.Vertex = skyTopShader.GetAttributionLocation("Vertex");
            skyTopShader.EnableAttribute(topLocs.Vertex);
            topLocs.TexCoord = skyTopShader.GetAttributionLocation("TexCoordInp");
            skyTopShader.EnableAttribute(topLocs.TexCoord);
            topLocs.Camera = skyTopShader.GetUniformLocation("Camera");
            topLocs.Time = skyTopShader.GetUniformLocation("Time");
            topLocs.EyePos = skyTopShader.GetUniformLocation("EyePos");
            topLocs.SkyColorA = skyTopShader.GetUniformLocation("SkyColorA");
            topLocs.SkyColorB = skyTopShader.GetUniformLocation("SkyColorB");
            topLocs.CloudDensity = skyTopShader.GetUniformLocation("CloudDensity");
            skyTopShader.SetUniform("Perlin", 3);

            GL.UseProgram(0);
        }

        public void OnDraw(DayNight dayNight)
        {
            skyTopShader.Use();
            skyTopShader.SetUniform(topLocs.Time, time);
            skyTopShader.SetUniform(topLocs.SkyColorA, dayNight.SkyColorA);
            skyTopShader.SetUniform(topLocs.SkyColorB, dayNight.SkyColorB);
            skyTopShader.SetUniform(topLocs.CloudDensity, dayNight.CloudDensity);
            verticles.Bind();
            GL.VertexAttribPointer(topLocs.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
            texCoord.Bind();
            GL.VertexAttribPointer(topLocs.TexCoord, 2, VertexAttribPointerType.Float, false, 0, 0);
            indices.Bind();
            GL.DrawElements(BeginMode.Triangles, 6 * 5, DrawElementsType.UnsignedShort, 0);
            GL.UseProgram(0);
        }

        public void SetPos(Vector2 pos, Renderer3D rnd)
        {
            time++;
            var eyePos = new Vector3(-21.0F * pos.X, -20.0F, 21.0F * pos.Y);
            var modelview = Matrix4.CreateTranslation(new Vector3(21.0F * pos.X, 0, -21.0F * pos.Y));
            var transform = modelview * rnd.Camera * rnd.Projection;

            skyTopShader.Use();
            skyTopShader.SetUniform(topLocs.Camera, ref transform);
            skyTopShader.SetUniform(topLocs.EyePos, eyePos);
            GL.UseProgram(0);
        }

        private GLBuffer verticles = new GLBuffer(OpenTK.Graphics.ES20.BufferTarget.ArrayBuffer);
        private GLBuffer indices = new GLBuffer(OpenTK.Graphics.ES20.BufferTarget.ElementArrayBuffer);
        private GLBuffer texCoord = new GLBuffer(OpenTK.Graphics.ES20.BufferTarget.ArrayBuffer);

        private GLShader skyTopShader;
        private float time;

        private struct SkyTopShaderLocs
        {
            public int
                Camera,
                Vertex,
                TexCoord,
                EyePos,
                SkyColorA,
                SkyColorB,
                CloudDensity,
                Time;
        }

        private SkyTopShaderLocs topLocs;
    }
}