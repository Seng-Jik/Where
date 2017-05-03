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
            playerShader = new Lower.GLShader("2D_VS", "2D_Player");
            wallShader.Use();
            wallShaderLocs.attrVertex = wallShader.GetAttributionLocation("Vertex");
            wallShader.EnableAttribute(wallShaderLocs.attrVertex);
            wallShaderLocs.unifCamera = wallShader.GetUniformLocation("Camera");

            playerShader.Use();
            playerShaderLocs.attrVertex = wallShader.GetAttributionLocation("Vertex");
            playerShader.EnableAttribute(wallShaderLocs.attrVertex);
            playerShaderLocs.unifCamera = wallShader.GetUniformLocation("Camera");

            player = new Lower.GLBuffer(BufferTarget.ArrayBuffer);
        }

        public void OnDraw()
        {
            wallShader.Use();
            wallScene.VertexBuffer.Bind();
            wallScene.IndexBuffer.Bind();
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);
            wallScene.IndexBuffer.Bind();
            GL.DrawElements(BeginMode.Triangles, wallScene.VerticleSize, DrawElementsType.UnsignedShort, 0);

            playerShader.Use();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            player.Bind();
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

            short[] playerTriangle = { 0, 1, 2 };
            GL.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedShort, playerTriangle);
        }

        public void SetCamera(float angle, Vector2 pos)
        {
            
            var camera = Matrix4.Identity;

            camera *= Matrix4.CreateTranslation(-new Vector3(pos));
            var rotation = Matrix4.CreateRotationZ(angle / 180.0F * 3.1415926F);
            camera *= rotation;

            camera *= Matrix4.CreateOrthographicOffCenter(32, -32, 32, -32, -200, 200); 



            player.Bind();
            Vector2[] playerBuf =
            {
                pos-new Vector2(0.0f,0.5f),
                pos-new Vector2(0.5f,0.0f),
                pos+new Vector2(0.5f,0.0f)
            };

            //Console.WriteLine(pos);

            player.BufferData(3 * 2 * sizeof(float), playerBuf, BufferUsageHint.DynamicDraw);

            wallShader.Use();
            wallShader.SetUniform(wallShaderLocs.unifCamera, ref camera);
            playerShader.Use();
            wallShader.SetUniform(wallShaderLocs.unifCamera, ref camera);
        }

        public void SetWallBuffer(List<Point> wallPoints)
        {
            wallShader.Use();
            
            wallScene = WallGen.CreateWallBuffer(wallPoints);
            
        }

        SceneBuffer wallScene;

        Lower.GLShader wallShader;
        struct Shader2DLocs {
            public int attrVertex;
            public int unifCamera;
        }
        Shader2DLocs wallShaderLocs,playerShaderLocs;

        Lower.GLBuffer player;
        Lower.GLShader playerShader;
    }
}
