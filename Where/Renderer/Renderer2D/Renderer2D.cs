using System;
using System.Collections.Generic;
using System.Text;
using MapGen;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace Where.Renderer.Renderer2D
{
    public class Renderer2D : IRenderer
    {
        public Renderer2D()
        {
            wallShader = new Lower.GLShader("2D_VS", "2D_Wall");
            wallShader.Use();
            wallShaderLocs.attrVertex = wallShader.GetAttributionLocation("Vertex");
            wallShader.EnableAttribute(wallShaderLocs.attrVertex);
            wallShaderLocs.unifCamera = wallShader.GetUniformLocation("Camera");
        }

        public void OnDraw()
        {
            wallShader.Use();
            wallScene.VertexBuffer.Bind();
            wallScene.IndexBuffer.Bind();
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            wallScene.IndexBuffer.Bind();
            GL.DrawElements(BeginMode.Triangles, wallScene.VerticleSize, DrawElementsType.UnsignedShort, 0);
        }

        public void SetCamera(double angle, Vector2 pos)
        {
            wallShader.Use();
            var camera = Matrix4.CreateOrthographicOffCenter(0, 64, 64, 0, -200, 200);
            wallShader.SetUniform(wallShaderLocs.unifCamera, ref camera);
        }

        public void SetWallBuffer(List<Point> wallPoints)
        {
            wallShader.Use();
            
            wallScene = WallGen.CreateWallBuffer(wallPoints);
            
        }

        SceneBuffer wallScene;

        Lower.GLShader wallShader;
        struct WallShaderLocs {
            public int attrVertex;
            public int unifCamera;
        }
        WallShaderLocs wallShaderLocs;
    }
}
