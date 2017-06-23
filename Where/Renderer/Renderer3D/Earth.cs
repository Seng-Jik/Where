using OpenTK;
using OpenTK.Graphics.ES20;
using Where.Renderer.Lower;

namespace Where.Renderer.Renderer3D
{
    internal class Earth
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
                new Vector2(0,100),
                new Vector2(100,0),
                new Vector2(100,100),
                new Vector2(0,100),
                new Vector2(0,0),
                new Vector2(100,0),
            };
            earthBuffer.Bind();
            earthBuffer.BufferData(3 * 6 * sizeof(float), earth, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);
            texCoordBuffer.Bind();
            texCoordBuffer.BufferData(2 * 6 * sizeof(float), texCoord, OpenTK.Graphics.ES20.BufferUsageHint.StaticDraw);
        }

        public void OnDraw(GLShader shader, Renderer3D.ObjectDrawShaderLocs locs)
        {
            shader.SetUniform(locs.Normal, new Vector3(0, 1, 0));
            Matrix3 tbn = Renderer3D.GetTBNMatrix(new Vector3(0, 1, 0), new Vector3(1, 0, 1));
            shader.SetUniform(locs.TBNMatrix, ref tbn);
            earthBuffer.Bind();
            GL.VertexAttribPointer(locs.Vertex, 3, VertexAttribPointerType.Float, false, 0, 0);
            texCoordBuffer.Bind();
            GL.VertexAttribPointer(locs.TexCoord, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        private GLBuffer earthBuffer = new GLBuffer(BufferTarget.ArrayBuffer);
        private GLBuffer texCoordBuffer = new GLBuffer(BufferTarget.ArrayBuffer);
    }
}